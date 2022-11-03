namespace MonkMailService.Models
{
    /** 
        <summary> This view model represents required fields only for user request. </summary> 
    */
    public class MailView
    {
        /** 
        <summary> Short content of the letter </summary> */
        public string Subject { get; set; } = string.Empty;
        /** 
        <summary> Main body of the letter with text, can me fileld using HTML tags </summary> */
        public string Body { get; set; } = string.Empty;
        /** 
        <summary> One or more recipients of the letter that will be sent to  </summary> */
        public string[] Recipients { get; set; }
    }
}
