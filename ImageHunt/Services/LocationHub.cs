using System;
using System.Threading.Tasks;
using ImageHuntCore.Model;
using Microsoft.AspNetCore.SignalR;

namespace ImageHunt.Services
{
  public class LocationHub : Hub, ILocationHub
  {
    public async Task PositionChanged(Team team, DateTime dateOccured, LatLng newPosition)
    {
      if (Clients != null)
        await Clients.All.SendAsync("PositionChanged", team, dateOccured, newPosition);
    }

    public async Task InitConnection()
    {
      await Clients.All.SendAsync("ConnectionInit");
    }

    public override Task OnConnectedAsync()
    {
      return base.OnConnectedAsync();
    }
  }

  public interface ILocationHub
  {
    Task PositionChanged(Team team, DateTime dateOccured, LatLng newPosition);
  }
}
