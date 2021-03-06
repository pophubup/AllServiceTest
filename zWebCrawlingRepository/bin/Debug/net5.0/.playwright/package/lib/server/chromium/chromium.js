"use strict";

Object.defineProperty(exports, "__esModule", {
  value: true
});
exports.Chromium = void 0;

var _fs = _interopRequireDefault(require("fs"));

var _os = _interopRequireDefault(require("os"));

var _path = _interopRequireDefault(require("path"));

var _crBrowser = require("./crBrowser");

var _crConnection = require("./crConnection");

var _stackTrace = require("../../utils/stackTrace");

var _browserType = require("../browserType");

var _transport = require("../transport");

var _crDevTools = require("./crDevTools");

var _utils = require("../../utils/utils");

var _debugLogger = require("../../utils/debugLogger");

var _progress = require("../progress");

var _timeoutSettings = require("../../utils/timeoutSettings");

var _helper = require("../helper");

var _http = _interopRequireDefault(require("http"));

var _registry = require("../../utils/registry");

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

/**
 * Copyright 2017 Google Inc. All rights reserved.
 * Modifications copyright (c) Microsoft Corporation.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
const ARTIFACTS_FOLDER = _path.default.join(_os.default.tmpdir(), 'playwright-artifacts-');

class Chromium extends _browserType.BrowserType {
  constructor(playwrightOptions) {
    super('chromium', playwrightOptions);
    this._devtools = void 0;
    if ((0, _utils.debugMode)()) this._devtools = this._createDevTools();
  }

  async connectOverCDP(metadata, endpointURL, options, timeout) {
    const controller = new _progress.ProgressController(metadata, this);
    controller.setLogName('browser');
    const browserLogsCollector = new _debugLogger.RecentLogsCollector();
    return controller.run(async progress => {
      let headersMap;
      if (options.headers) headersMap = (0, _utils.headersArrayToObject)(options.headers, false);
      const artifactsDir = await _fs.default.promises.mkdtemp(ARTIFACTS_FOLDER);
      const chromeTransport = await _transport.WebSocketTransport.connect(progress, await urlToWSEndpoint(endpointURL), headersMap);
      const browserProcess = {
        close: async () => {
          await (0, _utils.removeFolders)([artifactsDir]);
          await chromeTransport.closeAndWait();
        },
        kill: async () => {
          await (0, _utils.removeFolders)([artifactsDir]);
          await chromeTransport.closeAndWait();
        }
      };
      const browserOptions = { ...this._playwrightOptions,
        slowMo: options.slowMo,
        name: 'chromium',
        isChromium: true,
        persistent: {
          sdkLanguage: options.sdkLanguage,
          noDefaultViewport: true
        },
        browserProcess,
        protocolLogger: _helper.helper.debugProtocolLogger(),
        browserLogsCollector,
        artifactsDir,
        downloadsPath: artifactsDir,
        tracesDir: artifactsDir
      };
      return await _crBrowser.CRBrowser.connect(chromeTransport, browserOptions);
    }, _timeoutSettings.TimeoutSettings.timeout({
      timeout
    }));
  }

  _createDevTools() {
    // TODO: this is totally wrong when using channels.
    const directory = _registry.registry.findExecutable('chromium').directory;

    return directory ? new _crDevTools.CRDevTools(_path.default.join(directory, 'devtools-preferences.json')) : undefined;
  }

  async _connectToTransport(transport, options) {
    let devtools = this._devtools;

    if (options.__testHookForDevTools) {
      devtools = this._createDevTools();
      await options.__testHookForDevTools(devtools);
    }

    return _crBrowser.CRBrowser.connect(transport, options, devtools);
  }

  _rewriteStartupError(error) {
    // These error messages are taken from Chromium source code as of July, 2020:
    // https://github.com/chromium/chromium/blob/70565f67e79f79e17663ad1337dc6e63ee207ce9/content/browser/zygote_host/zygote_host_impl_linux.cc
    if (!error.message.includes('crbug.com/357670') && !error.message.includes('No usable sandbox!') && !error.message.includes('crbug.com/638180')) return error;
    return (0, _stackTrace.rewriteErrorMessage)(error, [`Chromium sandboxing failed!`, `================================`, `To workaround sandboxing issues, do either of the following:`, `  - (preferred): Configure environment to support sandboxing: https://github.com/microsoft/playwright/blob/master/docs/troubleshooting.md`, `  - (alternative): Launch Chromium without sandbox using 'chromiumSandbox: false' option`, `================================`, ``].join('\n'));
  }

  _amendEnvironment(env, userDataDir, executable, browserArguments) {
    return env;
  }

  _attemptToGracefullyCloseBrowser(transport) {
    const message = {
      method: 'Browser.close',
      id: _crConnection.kBrowserCloseMessageId,
      params: {}
    };
    transport.send(message);
  }

  _defaultArgs(options, isPersistent, userDataDir) {
    const {
      args = [],
      proxy
    } = options;
    const userDataDirArg = args.find(arg => arg.startsWith('--user-data-dir'));
    if (userDataDirArg) throw new Error('Pass userDataDir parameter to `browserType.launchPersistentContext(userDataDir, ...)` instead of specifying --user-data-dir argument');
    if (args.find(arg => arg.startsWith('--remote-debugging-pipe'))) throw new Error('Playwright manages remote debugging connection itself.');
    if (args.find(arg => !arg.startsWith('-'))) throw new Error('Arguments can not specify page to be opened');
    const chromeArguments = [...DEFAULT_ARGS];
    chromeArguments.push(`--user-data-dir=${userDataDir}`); // See https://github.com/microsoft/playwright/issues/7362

    if (_os.default.platform() === 'darwin') chromeArguments.push('--enable-use-zoom-for-dsf=false');
    if (options.useWebSocket) chromeArguments.push('--remote-debugging-port=0');else chromeArguments.push('--remote-debugging-pipe');
    if (options.devtools) chromeArguments.push('--auto-open-devtools-for-tabs');

    if (options.headless) {
      chromeArguments.push('--headless', '--hide-scrollbars', '--mute-audio', '--blink-settings=primaryHoverType=2,availableHoverTypes=2,primaryPointerType=4,availablePointerTypes=4');
    }

    if (options.chromiumSandbox !== true) chromeArguments.push('--no-sandbox');

    if (proxy) {
      const proxyURL = new URL(proxy.server);
      const isSocks = proxyURL.protocol === 'socks5:'; // https://www.chromium.org/developers/design-documents/network-settings

      if (isSocks) {
        // https://www.chromium.org/developers/design-documents/network-stack/socks-proxy
        chromeArguments.push(`--host-resolver-rules="MAP * ~NOTFOUND , EXCLUDE ${proxyURL.hostname}"`);
      }

      chromeArguments.push(`--proxy-server=${proxy.server}`);
      const proxyBypassRules = []; // https://source.chromium.org/chromium/chromium/src/+/master:net/docs/proxy.md;l=548;drc=71698e610121078e0d1a811054dcf9fd89b49578

      if (this._playwrightOptions.loopbackProxyOverride) proxyBypassRules.push('<-loopback>');
      if (proxy.bypass) proxyBypassRules.push(...proxy.bypass.split(',').map(t => t.trim()).map(t => t.startsWith('.') ? '*' + t : t));
      if (proxyBypassRules.length > 0) chromeArguments.push(`--proxy-bypass-list=${proxyBypassRules.join(';')}`);
    }

    chromeArguments.push(...args);
    if (isPersistent) chromeArguments.push('about:blank');else chromeArguments.push('--no-startup-window');
    return chromeArguments;
  }

}

exports.Chromium = Chromium;
const DEFAULT_ARGS = ['--disable-background-networking', '--enable-features=NetworkService,NetworkServiceInProcess', '--disable-background-timer-throttling', '--disable-backgrounding-occluded-windows', '--disable-breakpad', '--disable-client-side-phishing-detection', '--disable-component-extensions-with-background-pages', '--disable-default-apps', '--disable-dev-shm-usage', '--disable-extensions', '--disable-features=ImprovedCookieControls,LazyFrameLoading,GlobalMediaControls,DestroyProfileOnBrowserClose', '--allow-pre-commit-input', '--disable-hang-monitor', '--disable-ipc-flooding-protection', '--disable-popup-blocking', '--disable-prompt-on-repost', '--disable-renderer-backgrounding', '--disable-sync', '--force-color-profile=srgb', '--metrics-recording-only', '--no-first-run', '--enable-automation', '--password-store=basic', '--use-mock-keychain', // See https://chromium-review.googlesource.com/c/chromium/src/+/2436773
'--no-service-autorun'];

async function urlToWSEndpoint(endpointURL) {
  if (endpointURL.startsWith('ws')) return endpointURL;
  const httpURL = endpointURL.endsWith('/') ? `${endpointURL}json/version/` : `${endpointURL}/json/version/`;
  const json = await new Promise((resolve, reject) => {
    _http.default.get(httpURL, resp => {
      if (resp.statusCode < 200 || resp.statusCode >= 400) {
        reject(new Error(`Unexpected status ${resp.statusCode} when connecting to ${httpURL}.\n` + `This does not look like a DevTools server, try connecting via ws://.`));
      }

      let data = '';
      resp.on('data', chunk => data += chunk);
      resp.on('end', () => resolve(data));
    }).on('error', reject);
  });
  return JSON.parse(json).webSocketDebuggerUrl;
}