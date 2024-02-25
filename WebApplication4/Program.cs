using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("info.json");

Library library = builder.Configuration.GetSection("Library").Get<Library>() ?? new Library();

var app = builder.Build();

Profile myProfile = new Profile();
myProfile.Name = "Iryna";
myProfile.Surname = "Shavrina";

app.MapGet("/lib", async (http) =>
{
    http.Response.ContentType = "text/html";
    await http.Response.WriteAsync(@"
        <html>
        <body>
            <h1>Greetings:)</h1>
            <ul>
                <li><a href=""/lib/books"">Books</a></li>
                <li><a href=""/lib/profile"">Profile</a></li>
            </ul>
        </body>
        </html>
    ");
});

app.MapGet("/lib/books", () =>
{
    StringBuilder stringBuilder = new StringBuilder();
    foreach (var book in library.books)
    {
        stringBuilder.Append($"Book title is {book.Title}\nBook author is {book.Author}\n -------- \n");
    }
    return stringBuilder.ToString();
});

app.MapGet("/lib/profile/{id?}", (int? id) =>
{
    Profile profile;
    if (id is null || id < 0 || id > 5)
    {
        profile = myProfile;
    }
    else
    {
        profile = library.profiles[(int)id];
    }
    return $"Person name: {profile.Name}\nPerson surname: {profile.Surname}";
});

app.Run();
