using CourtReservation_Core.CustomEntities;
using CourtReservation_Infraestructure.Dto_s;

namespace CourtReservation_Api.Responses
{
    public class ApiResponse<T>
    {
       

        public T Data { get; set; }

        public Pagination Pagination { get; set; }
        public Message[] Messages { get; set; }
        public ApiResponse(T data)
        {
            Data = data;
        }
        public string Message { get; set; } 
        public bool Success { get; set; }  
        public int StatusCode { get; set; } 
    }

}