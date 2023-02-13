using Common.Contract.DataTransferObjects;

namespace Common.Contract.RequestsAndResponses
{
    public class CreateRoomRequest
    {
        public CreateRoomRequest()
        {

        }
        public CreateRoomRequest(CreateRoomDTO createRoomDTO)
        {
            CreateRoom = createRoomDTO;
        }

        public CreateRoomDTO CreateRoom { get; set; }


    }

    public class CreateRoomResponse : BaseResponse
    {

    }
}
