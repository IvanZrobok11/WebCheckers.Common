namespace Common.Contract.DataTransferObjects
{
    public class CreateRoomDTO
    {
        public CreateRoomDTO()
        {

        }
        public CreateRoomDTO(int roomId, int userId)
        {
            UserId = userId;
            RoomId = roomId;
        }

        public int RoomId { get; set; }
        public int UserId { get; set; }
    }
}
