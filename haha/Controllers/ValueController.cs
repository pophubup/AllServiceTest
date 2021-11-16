using haha.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Reactive.Linq;

namespace haha.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ValueController : ControllerBase
    {
        private IHubContext<ChatHub> _hubContext;
        private IDisposable subscription;
        //private HostedBroadcaster _hostedBroadcaster;
        public ValueController(IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;

            //_hostedBroadcaster = new HostedBroadcaster(_hubContext);
            
        }
        [HttpPost]
        public async Task<IActionResult> start([FromBody] List<string> vs)
        {
            List<string> vs1 = new List<string>();
            vs1.AddRange(vs);
            //_hostedBroadcaster.paths = vs;
            //_hostedBroadcaster.StartAsync(new CancellationToken());
            subscription = Observable.Interval(TimeSpan.FromSeconds(10)).Subscribe(_ =>
            {
               
                Parallel.ForEach(vs1, item =>
                {
                    //var memoryStream = new MemoryStream();
                    //var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true);
                    //var demoFile = archive.CreateEntry($"foo_{item}.txt");
                    //var entryStream = demoFile.Open();
                    //var streamWriter = new StreamWriter(entryStream);
                    
                    _hubContext.Clients.All.SendAsync("broadcastMessage", DateTimeOffset.UtcNow, item);
                });

            });
            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> stop()
        {
            // _hostedBroadcaster.StopAsync(new CancellationToken());
            subscription?.Dispose();
            return Ok();
        }
    }
}
