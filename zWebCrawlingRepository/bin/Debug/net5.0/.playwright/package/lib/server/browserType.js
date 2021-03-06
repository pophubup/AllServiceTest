"use strict";

Object.defineProperty(exports, "__esModule", {
  value: true
});
exports.BrowserType = void 0;

var _fs = _interopRequireDefault(require("fs"));

var os = _interopRequireWildcard(require("os"));

var _path = _interopRequireDefault(require("path"));

var _browserContext = require("./browserContext");

var _registry = require("../utils/registry");

var _transport = require("./transport");

var _processLauncher = require("../utils/processLauncher");

var _pipeTransport = require("./pipeTransport");

var _progress = require("./progress");

var _timeoutSettings = require("../utils/timeoutSettings");

var _utils = require("../utils/utils");

var _helper = require("./helper");

var _debugLogger = require("../utils/debugLogger");

var _instrumentation = require("./instrumentation");

function _getRequireWildcardCache(nodeInterop) { if (typeof WeakMap !== "function") return null; var cacheBabelInterop = new WeakMap(); var cacheNodeInterop = new WeakMap(); return (_getRequireWildcardCache = function (nodeInterop) { return nodeInterop ? cacheNodeInterop : cacheBabelInterop; })(nodeInterop); }

function _interopRequireWildcard(obj, nodeInterop) { if (!nodeInterop && obj && obj.__esModule) { return obj; } if (obj === null || typeof obj !== "object" && typeof obj !== "function") { return { default: obj }; } var cache = _getRequireWildcardCache(nodeInterop); if (cache && cache.has(obj)) { return cache.get(obj); } var newObj = {}; var hasPropertyDescriptor = Object.defineProperty && Object.getOwnPropertyDescriptor; for (var key in obj) { if (key !== "default" && Object.prototype.hasOwnProperty.call(obj, key)) { var desc = hasPropertyDescriptor ? Object.getOwnPropertyDescriptor(obj, key) : null; if (desc && (desc.get || desc.set)) { Object.defineProperty(newObj, key, desc); } else { newObj[key] = obj[key]; } } } newObj.default = obj; if (cache) { cache.set(obj, newObj); } return newObj; }

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

/**
 * Copyright (c) Microsoft Corporation.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
const ARTIFACTS_FOLDER = _path.default.join(os.tmpdir(), 'playwright-artifacts-');

class BrowserType extends _instrumentation.SdkObject {
  constructor(browserName, playwrightOptions) {
    super(playwrightOptions.rootSdkObject, 'browser-type');
    this._name = void 0;
    this._playwrightOptions = void 0;
    this.attribution.browserType = this;
    this._playwrightOptions = playwrightOptions;
    this._name = browserName;
  }

  executablePath() {
    return _registry.registry.findExecutable(this._name).executablePath() || '';
  }

  name() {
    return this._name;
  }

  async launch(metadata, options, protocolLogger) {
    var _this$_playwrightOpti, _this$_playwrightOpti2;

    options = validateLaunchOptions(options, (_this$_playwrightOpti = (_this$_playwrightOpti2 = this._playwrightOptions).loopbackProxyOverride) === null || _this$_playwrightOpti === void 0 ? void 0 : _this$_playwrightOpti.call(_this$_playwrightOpti2));
    const controller = new _progress.ProgressController(metadata, this);
    controller.setLogName('browser');
    const browser = await controller.run(progress => {
      return this._innerLaunchWithRetries(progress, options, undefined, _helper.helper.debugProtocolLogger(protocolLogger)).catch(e => {
        throw this._rewriteStartupError(e);
      });
    }, _timeoutSettings.TimeoutSettings.timeout(options));
    return browser;
  }

  async launchPersistentContext(metadata, userDataDir, options) {
    var _this$_playwrightOpti3, _this$_playwrightOpti4;

    options = validateLaunchOptions(options, (_this$_playwrightOpti3 = (_this$_playwrightOpti4 = this._playwrightOptions).loopbackProxyOverride) === null || _this$_playwrightOpti3 === void 0 ? void 0 : _this$_playwrightOpti3.call(_this$_playwrightOpti4));
    const controller = new _progress.ProgressController(metadata, this);
    const persistent = options;
    controller.setLogName('browser');
    const browser = await controller.run(progress => {
      return this._innerLaunchWithRetries(progress, options, persistent, _helper.helper.debugProtocolLogger(), userDataDir).catch(e => {
        throw this._rewriteStartupError(e);
      });
    }, _timeoutSettings.TimeoutSettings.timeout(options));
    return browser._defaultContext;
  }

  async _innerLaunchWithRetries(progress, options, persistent, protocolLogger, userDataDir) {
    try {
      return this._innerLaunch(progress, options, persistent, protocolLogger, userDataDir);
    } catch (error) {
      // @see https://github.com/microsoft/playwright/issues/5214
      const errorMessage = typeof error === 'object' && typeof error.message === 'string' ? error.message : '';

      if (errorMessage.includes('Inconsistency detected by ld.so')) {
        progress.log(`<restarting browser due to hitting race condition in glibc>`);
        return this._innerLaunch(progress, options, persistent, protocolLogger, userDataDir);
      }

      throw error;
    }
  }

  async _innerLaunch(progress, options, persistent, protocolLogger, userDataDir) {
    options.proxy = options.proxy ? (0, _browserContext.normalizeProxySettings)(options.proxy) : undefined;
    const browserLogsCollector = new _debugLogger.RecentLogsCollector();
    const {
      browserProcess,
      artifactsDir,
      transport
    } = await this._launchProcess(progress, options, !!persistent, browserLogsCollector, userDataDir);
    if (options.__testHookBeforeCreateBrowser) await options.__testHookBeforeCreateBrowser();
    const browserOptions = { ...this._playwrightOptions,
      name: this._name,
      isChromium: this._name === 'chromium',
      channel: options.channel,
      slowMo: options.slowMo,
      persistent,
      headful: !options.headless,
      artifactsDir,
      downloadsPath: options.downloadsPath || artifactsDir,
      tracesDir: options.tracesDir || artifactsDir,
      browserProcess,
      customExecutablePath: options.executablePath,
      proxy: options.proxy,
      protocolLogger,
      browserLogsCollector,
      wsEndpoint: options.useWebSocket ? transport.wsEndpoint : undefined
    };
    if (persistent) (0, _browserContext.validateBrowserContextOptions)(persistent, browserOptions);
    copyTestHooks(options, browserOptions);
    const browser = await this._connectToTransport(transport, browserOptions); // We assume no control when using custom arguments, and do not prepare the default context in that case.

    if (persistent && !options.ignoreAllDefaultArgs) await browser._defaultContext._loadDefaultContext(progress);
    return browser;
  }

  async _launchProcess(progress, options, isPersistent, browserLogsCollector, userDataDir) {
    var _options$args;

    const {
      ignoreDefaultArgs,
      ignoreAllDefaultArgs,
      args = [],
      executablePath = null,
      handleSIGINT = true,
      handleSIGTERM = true,
      handleSIGHUP = true
    } = options;
    const env = options.env ? (0, _processLauncher.envArrayToObject)(options.env) : process.env;
    const tempDirectories = [];
    if (options.downloadsPath) await _fs.default.promises.mkdir(options.downloadsPath, {
      recursive: true
    });
    if (options.tracesDir) await _fs.default.promises.mkdir(options.tracesDir, {
      recursive: true
    });
    const artifactsDir = await _fs.default.promises.mkdtemp(ARTIFACTS_FOLDER);
    tempDirectories.push(artifactsDir);

    if (!userDataDir) {
      userDataDir = await _fs.default.promises.mkdtemp(_path.default.join(os.tmpdir(), `playwright_${this._name}dev_profile-`));
      tempDirectories.push(userDataDir);
    }

    const browserArguments = [];
    if (ignoreAllDefaultArgs) browserArguments.push(...args);else if (ignoreDefaultArgs) browserArguments.push(...this._defaultArgs(options, isPersistent, userDataDir).filter(arg => ignoreDefaultArgs.indexOf(arg) === -1));else browserArguments.push(...this._defaultArgs(options, isPersistent, userDataDir));
    let executable;

    if (executablePath) {
      if (!(await (0, _utils.existsAsync)(executablePath))) throw new Error(`Failed to launch ${this._name} because executable doesn't exist at ${executablePath}`);
      executable = executablePath;
    } else {
      const registryExecutable = _registry.registry.findExecutable(options.channel || this._name);

      if (!registryExecutable || registryExecutable.browserName !== this._name) throw new Error(`Unsupported ${this._name} channel "${options.channel}"`);
      executable = registryExecutable.executablePathOrDie();
      await registryExecutable.validateHostRequirements();
    }

    let wsEndpointCallback;
    const shouldWaitForWSListening = options.useWebSocket || ((_options$args = options.args) === null || _options$args === void 0 ? void 0 : _options$args.some(a => a.startsWith('--remote-debugging-port')));
    const waitForWSEndpoint = shouldWaitForWSListening ? new Promise(f => wsEndpointCallback = f) : undefined; // Note: it is important to define these variables before launchProcess, so that we don't get
    // "Cannot access 'browserServer' before initialization" if something went wrong.

    let transport = undefined;
    let browserProcess = undefined;
    const {
      launchedProcess,
      gracefullyClose,
      kill
    } = await (0, _processLauncher.launchProcess)({
      command: executable,
      args: browserArguments,
      env: this._amendEnvironment(env, userDataDir, executable, browserArguments),
      handleSIGINT,
      handleSIGTERM,
      handleSIGHUP,
      log: message => {
        if (wsEndpointCallback) {
          const match = message.match(/DevTools listening on (.*)/);
          if (match) wsEndpointCallback(match[1]);
        }

        progress.log(message);
        browserLogsCollector.log(message);
      },
      stdio: 'pipe',
      tempDirectories,
      attemptToGracefullyClose: async () => {
        if (options.__testHookGracefullyClose) await options.__testHookGracefullyClose(); // We try to gracefully close to prevent crash reporting and core dumps.
        // Note that it's fine to reuse the pipe transport, since
        // our connection ignores kBrowserCloseMessageId.

        this._attemptToGracefullyCloseBrowser(transport);
      },
      onExit: (exitCode, signal) => {
        if (browserProcess && browserProcess.onclose) browserProcess.onclose(exitCode, signal);
      }
    });

    async function closeOrKill(timeout) {
      let timer;

      try {
        await Promise.race([gracefullyClose(), new Promise((resolve, reject) => timer = setTimeout(reject, timeout))]);
      } catch (ignored) {
        await kill().catch(ignored => {}); // Make sure to await actual process exit.
      } finally {
        clearTimeout(timer);
      }
    }

    browserProcess = {
      onclose: undefined,
      process: launchedProcess,
      close: () => closeOrKill(options.__testHookBrowserCloseTimeout || _timeoutSettings.DEFAULT_TIMEOUT),
      kill
    };
    progress.cleanupWhenAborted(() => closeOrKill(progress.timeUntilDeadline()));
    let wsEndpoint;
    if (shouldWaitForWSListening) wsEndpoint = await waitForWSEndpoint;

    if (options.useWebSocket) {
      transport = await _transport.WebSocketTransport.connect(progress, wsEndpoint);
    } else {
      const stdio = launchedProcess.stdio;
      transport = new _pipeTransport.PipeTransport(stdio[3], stdio[4]);
    }

    return {
      browserProcess,
      artifactsDir,
      transport
    };
  }

  async connectOverCDP(metadata, endpointURL, options, timeout) {
    throw new Error('CDP connections are only supported by Chromium');
  }

}

exports.BrowserType = BrowserType;

function copyTestHooks(from, to) {
  for (const [key, value] of Object.entries(from)) {
    if (key.startsWith('__testHook')) to[key] = value;
  }
}

function validateLaunchOptions(options, proxyOverride) {
  const {
    devtools = false
  } = options;
  let {
    headless = !devtools,
    downloadsPath,
    proxy
  } = options;
  if ((0, _utils.debugMode)()) headless = false;
  if (downloadsPath && !_path.default.isAbsolute(downloadsPath)) downloadsPath = _path.default.join(process.cwd(), downloadsPath);
  if (proxyOverride) proxy = {
    server: proxyOverride
  };
  return { ...options,
    devtools,
    headless,
    downloadsPath,
    proxy
  };
}