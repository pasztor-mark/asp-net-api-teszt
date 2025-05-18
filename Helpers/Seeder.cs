using api_teszt.Database;
using api_teszt.Enums;
using api_teszt.Models;
using Bogus;

namespace api_teszt.Helpers;

public static class Seeder
{
    public static void SeedDatabase(DatabaseContext ctx)
    {
        if (ctx.users.Any()) return; // ha van valami a táblában, hagyja abba
        Faker<User> users = new Faker<User>()
            .RuleFor(u => u.first_name, f => f.Name.FirstName())
            .RuleFor(u => u.last_name, f => f.Name.LastName())
            .RuleFor(u => u.email, f => f.Internet.Email())
            .RuleFor(u => u.bio, f => f.Lorem.Sentences())
            .RuleFor(u => u.notes, (f, u) =>
            {
                return new Faker<Note>()
                    .RuleFor(n => n.note_content, f => f.Lorem.Paragraph())
                    .RuleFor(n => n.author, _ => u)
                    .RuleFor(n => n.author_id, _ => u.id)
                    .RuleFor(n => n.tags, f => f.PickRandom<Tag>(Enum.GetValues<Tag>(), f.Random.Int(1, 3)).ToList())
                    .Generate(5);
            });


        List<User> userList = users.Generate(10);
        ctx.users.AddRange(userList);
        ctx.SaveChanges();
    }
}