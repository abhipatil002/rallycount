using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace BadmintonApi.Hubs;

[Authorize]
public class BadmintonHub : Hub
{
    public async Task JoinPeriod(string periodId) =>
        await Groups.AddToGroupAsync(Context.ConnectionId, periodId);

    public async Task LeavePeriod(string periodId) =>
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, periodId);
}
