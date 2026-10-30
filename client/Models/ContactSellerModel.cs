using System.ComponentModel.DataAnnotations;

namespace client.Models;

public class ContactSellerModel
{
    [Required(ErrorMessage = "Name is required.")]
    [StringLength(50, ErrorMessage = "Name must be at most 50 characters.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Enter a valid email address.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Message is required.")]
    [StringLength(500, ErrorMessage = "Message must be at most 500 characters.")]
    public string Message { get; set; } = string.Empty;

    [Range(0.0, 100000.0, ErrorMessage = "Offer must be between 0 and 100000.")]
    public decimal? OfferPrice { get; set; }
}
