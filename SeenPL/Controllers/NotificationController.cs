using Microsoft.AspNetCore.Mvc;
using SeenCL.DTOs;
using SeenCL.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SeenAPI.Controllers
{
    /// <summary>
    /// Controller for handling user notifications and marking them as read.
    /// </summary>
    [Route("api/notifications")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        /// <summary>
        /// WHO: Mobile App / All Users.
        /// WHAT: Retrieves all notifications for a specific user.
        /// </summary>
        [HttpGet("user/{userId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<NotificationDTO>>> GetUserNotifications(int userId)
        {
            var notifications = await _notificationService.GetUserNotificationsAsync(userId);
            return Ok(notifications);
        }

        /// <summary>
        /// WHO: Mobile App.
        /// WHAT: Marks a specific notification as read.
        /// </summary>
        [HttpPost("{id:int}/read")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> MarkRead(int id)
        {
            if (await _notificationService.MarkAsReadAsync(id)) return Ok(new { message = "Notification marked as read" });
            return BadRequest(new { message = "Failed to update notification status" });
        }
    }
}
