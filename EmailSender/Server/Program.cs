using EmailSender.Data.EmailContext;
using EmailSender.Models;
using EmailSender.Services;
using MailKit;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddDbContextPool<EmailDbContext>(options =>
//	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContextFactory<EmailDbContext>(
	   options =>
		   options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddTransient<IEmailService,EmailService>();
builder.Services.AddCors(options => options.AddPolicy(name: "NgOrigins", policy =>
{
	policy.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader();
	policy.AllowAnyOrigin();
}));

builder.Services.AddCors(options =>
{
	options.AddDefaultPolicy(
		builder =>
		{
			builder
				.AllowAnyHeader()
				.AllowAnyMethod()
				.AllowAnyOrigin();
		});
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
	app.UseDeveloperExceptionPage();
}
else if (app.Environment.IsProduction()) {  app.UseSwaggerUI(); app.UseSwagger();}

app.UseCors();
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
