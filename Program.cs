using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connection = String.Empty;
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddEnvironmentVariables().AddJsonFile("appsettings.Development.json");
    connection = builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING");
}
else
{
    connection = Environment.GetEnvironmentVariable("AZURE_SQL_CONNECTIONSTRING");
}
builder.Services.AddDbContext<BlogContext>(options => options.UseSqlServer(connection));

var app = builder.Build();

app.MapGet("https://vetlelgwebapi.azurewebsites.net/", () => "Hello World!");

app.MapGet("https://vetlelgwebapi.azurewebsites.net/Blog", (BlogContext context) => context.Blogs.ToList())
.WithName("GetBlogs");

app.MapPost("https://vetlelgwebapi.azurewebsites.net/Blog", (Blog blog, BlogContext context) =>
{
    context.Add(blog);
    context.SaveChanges();
})
.WithName("CreateBlog");

app.MapPost("https://vetlelgwebapi.azurewebsites.net/Post", (Post post, BlogContext context) =>
{
    context.Add(post);
    context.SaveChanges();
})
.WithName("CreatePost");

/*app.MapDelete("/Blog", (Blog blog, BlogContext context) => 
{
    context.Remove(blog);
    context.SaveChanges();
})
.WithName("DeleteBlog");

app.MapDelete("/Post", (Post post, BlogContext context) => 
{
    context.Remove(post);
    context.SaveChanges();
})
.WithName("DeletePost");*/

app.Run();

