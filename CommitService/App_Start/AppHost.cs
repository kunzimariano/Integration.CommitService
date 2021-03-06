using CommitService.Contract;
using Infrastructure.Composition;
using ServiceStack.Common.Utils;
using ServiceStack.Messaging;
using ServiceStack.Redis;
using ServiceStack.Redis.Messaging;
using ServiceStack.WebHost.Endpoints;
using ServiceStack.WebHost.Endpoints.Extensions;

[assembly: WebActivator.PreApplicationStartMethod(typeof(CommitService.App_Start.AppHost), "Start")]

namespace CommitService.App_Start
{
    public class AppHost
        : AppHostBase
    {
        public AppHost()
            : base("Commit Service", typeof(CommitService).Assembly) { }

        public override void Configure(Funq.Container container)
        {
            PathProvider.BinaryPath = "~".MapAbsolutePath();

            ServiceStack.Text.JsConfig.EmitCamelCaseNames = true;

			RequestBinders.Add(typeof(CommitAttempt), request => new CommitAttempt()
			{
				UserAgent = request.Headers["User-Agent"],
				RawBody = request.GetRawBody()
			});
			

            Routes
                .Add<CommitAttempt>("/commit")
                .Add<CommitMessages>("/commits")
                .Add<MessageErrors>("/errors")

            //    //.Add<CommitMessage>("/commitMessage")
              ;

            var redisFactory = new PooledRedisClientManager("localhost:6379");
            container.Register<IRedisClientsManager>(redisFactory);
            //var mqHost = new RedisMqHost(redisFactory);
            var mqHost = new RedisMqServer(redisFactory);

            container.Register<IMessageService>(mqHost);
            container.Register(mqHost.MessageFactory);

            mqHost.RegisterHandler<CommitAttempt>(ServiceController.ExecuteMessage);
            //mqHost.RegisterHandler<CommitMessage>(ServiceController.ExecuteMessage);

            mqHost.Start();
        }

        public static void Start()
        {
            new AppHost().Init();
        }
    }
}
