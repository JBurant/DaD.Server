namespace Common.DTO
{
    public enum ErrorCode
    {
        IE0000, // Invalid File format
        IE0001, // File already exists
        IE0002, // File does not exist
        IE0003, // User creation failed
        IE0004, // Login Password incorrect
        IE0005, // User does not exist
        IE0006, // User already exists
        IE0007, // Invalid form of username
        IE0008, // Invalid form of password
        IE0009, // Invalid form of email
        IE0010, // Unable to delete article
        IE0011, // Unable to write article
    };
}