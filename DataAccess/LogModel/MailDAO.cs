using System;
using System.ComponentModel.DataAnnotations;

namespace MailLog.LogModel
{
    /** 
        <summary> Model with all required fields for logging into SQLite DB. </summary> 
    */
    public class MailDAO
    {
        /**  
         <summary> Primary key for LoggedMail table </summary>
        */
        [Key]
        public int Id { get; set; }
        /** 
        <summary> Short content of the letter </summary> */
        [Required]
        public string Subject { get; set; }
        /** 
        <summary> Main body of the letter with text </summary> */
        [Required]
        public string Body { get; set; }
        /** 
        <summary> One or more recipients of the letter that will be sent to 
        SQLite cant store arrays so it will be processed as string</summary> */
        [Required]
        public string Recipients { get; set; }
        /** 
        <summary> Date and Time when logged message was formed  </summary> */
        [Required]
        public DateTime Date { get; set; }
        /** 
         <summary> The result of processed message. Ether OK or Failed. </summary> */
        [Required]
        public string Result { get; set; }
        /** 
        <summary> If Error during transaction accured, error message will be saved </summary> */
        public string? ErrorMessage { get; set; }
    }
}
