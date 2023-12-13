using genesis_apolices_processamento_teste.ConsomeArquivo;
using genesis_apolices_processamento_teste.EnviaApolice;
using genesis_apolices_processamento_teste.ProcessaApolice;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<KafkaConfig>(
    builder.Configuration.GetSection("KafkaConfig"));
builder.Services.Configure<SqsConfig>(
    builder.Configuration.GetSection("SqsConfig"));
builder.Services.AddHostedService<ConsomeSQS>();
builder.Services.AddSingleton<IProcessaApolice, ProcessaApolice>();
builder.Services.AddSingleton<IEnviaApolice, EnviaApoliceKafka>();

var app = builder.Build();
app.Run();
