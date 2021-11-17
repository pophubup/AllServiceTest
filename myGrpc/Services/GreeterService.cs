using Bogus;
using Bogus.Extensions;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace myGrpc.Services
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        private static int i = 0;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }
        public override Task<Candidate> GetCv(Empty request, ServerCallContext context)
        {
            var fakeJobs = new Faker<Job>()
  .RuleFor(a => a.Title, (f, u) => f.Company.Bs())
  .RuleFor(a => a.Salary, (f, u) => f.Commerce.Random.Int(1000, 2000))
  .RuleFor(a => a.JobDescription, (f, u) => f.Lorem.Text());
            var data = new Faker<Candidate>()
                .RuleFor(a => a.Name, (f, u) => f.Name.FullName())
                .RuleFor(a => a.Jobs, (f, u) =>
                {
                    u.Jobs.AddRange(fakeJobs.GenerateBetween(3, 5));
                    return u.Jobs;
                });
            var final = data.Generate();
            return Task.Run(() =>
            {
                return final;
            });
        }
        public override async Task<CreateCvResponse> CreateCv(IAsyncStreamReader<Candidate> requestStream, ServerCallContext context)
        {
            var result = new CreateCvResponse
            {
                IsSuccess = false
            };
            // stream Ū��
            while (await requestStream.MoveNext())
            {
                var candidate = requestStream.Current;
                // ��ڳB�z
                Console.WriteLine(candidate.Name);
            }
            return result;
        }
        public override async Task DownloadCv(DownloadByName request, IServerStreamWriter<Candidate> responseStream, ServerCallContext context)
        {
            while (i <= 100)
            {

                i++;
                var fakeJobs = new Faker<Job>()
      .RuleFor(a => a.Title, (f, u) => f.Company.Bs())
      .RuleFor(a => a.Salary, (f, u) => f.Commerce.Random.Int(1000, 2000))
      .RuleFor(a => a.JobDescription, (f, u) => f.Lorem.Text());
                var createRequests = new Faker<Candidate>()
                    .RuleFor(a => a.Name, (f, u) => f.Name.FullName())
                    .RuleFor(a => a.Jobs, (f, u) =>
                    {
                        u.Jobs.AddRange(fakeJobs.GenerateBetween(3, 5));
                        return u.Jobs;
                    }).Generate();
                // �N�C����Ƴv�@�z�L WriteAsync ��X
                await responseStream.WriteAsync(createRequests);
            }
        }
        public override async Task CreateDownloadCv(IAsyncStreamReader<Candidate> requestStream,
    IServerStreamWriter<Candidates> responseStream, ServerCallContext context)
        {
            var candidates = new Candidates();
            // �N���쪺��Ƴv�@���X
            while (await requestStream.MoveNext())
            {
                var candidate = requestStream.Current;
                candidates.Candidates_.Add(candidate);
                // �N�B�z�᪺��Ʀ^��
                await responseStream.WriteAsync(candidates);
            }
        }

    }

}
