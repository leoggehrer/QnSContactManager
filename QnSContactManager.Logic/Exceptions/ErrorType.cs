//@QnSCodeCopy
//MdStart
namespace QnSContactManager.Logic.Exceptions
{
    public enum ErrorType : int
    {
        InitAppAccess = 1,
        InvalidAccount = InitAppAccess * 2,
        NotLogedIn = InvalidAccount * 2,
        NotAuthorized = NotLogedIn * 2,
        InvalidToken = NotAuthorized * 2,
        InvalidId = InvalidToken * 2,
        InvalidPageSize = InvalidId * 2,
        InvalidSessionToken = InvalidPageSize * 2,
        InvalidJsonWebToken = InvalidSessionToken * 2,
        AuthorizationTimeOut = InvalidJsonWebToken * 2,
        InvalidEmail = AuthorizationTimeOut * 2,
        InvalidPassword = InvalidEmail * 2,
    }
}
//MdEnd
