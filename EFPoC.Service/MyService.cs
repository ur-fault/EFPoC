using EFPoC.DAL.Models;
using EFPoC.SL;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using LoremNET;

namespace EFPoC.Service;

internal class MyService : BackgroundService
{
    private readonly ILogger<MyService> _logger;
    private readonly UserService _userService;

    public MyService(ILogger<MyService> logger, UserService userService) {
        _logger = logger;
        _userService = userService;
    }

    protected override async Task ExecuteAsync(CancellationToken ct) {
        _logger.LogInformation("{Service} is starting.", nameof(MyService));

        var lastPurge = DateTimeOffset.Now;

        while (!ct.IsCancellationRequested) {
            var users = await _userService.GetUsersWithCommentsAsync(1, ct);

            var allIds = await _userService.GetAllIds(ct);

            foreach (var user in users)
                _logger.LogInformation("User {Id} has {Count} comments.", user.Id, user.Comments.Count);

            var email = Lorem.Email();
            await _userService.AddUserAsync(new User { Email = email }, ct);
            _logger.LogInformation("Added new user with email {Email}", email);

            if (allIds.Count > 0) {
                for (var rem = Random.Shared.Next(1, 5); rem > 0; rem--) {
                    var comment = new Comment {
                        Text = Lorem.Words(3, 20),
                        UserId = allIds[Random.Shared.Next(allIds.Count)],
                        CreatedAt = Lorem.DateTime(),
                        Likes = (int)Math.Pow(Random.Shared.Next(0, 100), 2),
                    };

                    await _userService.AddCommentAsync(comment, dontSave: true, ct: ct);
                }
            }

            await _userService.SaveChangesAsync(ct);

            if (DateTimeOffset.Now - lastPurge > TimeSpan.FromSeconds(30)) {
                await _userService.PurgeCommentsAsync(TimeSpan.FromSeconds(30), ct);
                lastPurge = DateTimeOffset.Now;
            }

            await Task.Delay(TimeSpan.FromSeconds(5), ct);
        }
    }
}
