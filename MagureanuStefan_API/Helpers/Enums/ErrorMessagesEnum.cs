namespace MagureanuStefan_API.Helpers.Enums
{
    public static class ErrorMessagesEnum
    {
        public static class Announcement
        {
            public const string NotFound = "No element found in the table!";
            public const string NotFoundById = "Announcement with given id doesn't exist";
            public const string BadRequest = "The entered format is wrong!";
            public const string ZeroUpdateToSave = "There is no modification to the announcement";
            public const string StartEndDatesError = "Data de final nu poate fi mai devreme decat data de inceput";
            public const string TitleExistsError = "Titlul exista deja in baza de date";
            public const string WrongFormatPut = "Body information isn't complete.";
        }
        public static class CodeSnippet
        {
            public const string WrongFormatPut = "Body information isn't complete.";
            public const string BadRequest = "The entered format is wrong!";
            public const string NotFound = "No element found in the table!";
            public const string NotFoundById = "CodeSnippet with given id doesn't exist";
            public const string ContentCodeError = "Exista deja un code snippet cu acest content";
            public const string MemberDoesntExist = "Id-ul pus pentru membru nu exista in baza de date";
            public const string ZeroUpdateToSave = "There is no modification to the codeSnippet";
        }
        public static class Member
        {
            public const string WrongFormatPut = "Body information isn't complete.";
            public const string BadRequest = "The entered format is wrong!";
            public const string NotFound = "No element found in the table!";
            public const string NotFoundById = "Member with given id doesn't exist";
            public const string UsernameExists = "There is another member with the given username";
            public const string ZeroUpdateToSave = "There is no modification to the member";
        }
        public static class MembershipType
        {
            public const string WrongFormatPut = "Body information isn't complete.";
            public const string BadRequest = "The entered format is wrong!";
            public const string NotFound = "No element found in the table!";
            public const string NotFoundById = "MembershipType with given id doesn't exist";
            public const string MembershipTypeExists = "This type of membership exists in database!";
            public const string ZeroUpdateToSave = "There is no modification to the membershipType";
        }
    }
}
