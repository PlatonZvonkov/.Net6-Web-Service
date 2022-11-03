namespace MonkMailService.Models
{
    /** 
        <summary> Model with all required fields for logging purposes. </summary> 
    */
    public class MailBody
    {
        /** 
        <summary> Short content of the letter </summary> */
        public string Subject { get; set; } = string.Empty;
        /** 
        <summary> Main body of the letter with text, can me fileld using HTML tags </summary> */
        public string Body { get; set; } = string.Empty;
        /** 
        <summary> One or more recipients of the letter that will be sent to </summary> */
        public string Recipients { get; set; } = string.Empty;
        /** 
        <summary> Date and Time when loggging was attempted </summary> */
        public DateTime Date { get; set; } = DateTime.Now;
        /** 
         <summary> The result of processed message. Ether OK or Failed. </summary> */
        public string? Result { get; set; }
        /** 
        <summary> If Error during transaction accured, error message will be saved,
         esle it will be Null</summary> */
        public string? ErrorMessage { get; set; }
    }    
}
