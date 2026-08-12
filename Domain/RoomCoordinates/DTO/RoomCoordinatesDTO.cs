namespace dddnet8.Domain.RoomCoordinates.DTO
{
    public class RoomCoordinatesDTO
    {
        /// <summary>
        /// Gets or sets the position (coordinate) of the room.
        /// </summary>
        public (int, int)? Position { get; set; }

        /// <summary>
        /// Gets or sets the size (width and length) of the room.
        /// </summary>
        public (int, int)? Size { get; set; }

        /// <summary>
        /// Gets or sets the direction of the door of the room.
        /// </summary>
        public int? DoorDirection { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoomCoordinatesDTO"/> class.
        /// </summary>
        public RoomCoordinatesDTO() {}

        /// <summary>
        /// Initializes a new instance of the <see cref="RoomCoordinatesDTO"/> class.
        /// </summary>
        /// <param name="position">The position of the room as a tuple (x, y).</param>
        /// <param name="size">The size of the room as a tuple (width, length).</param>
        /// <param name="doorDirection">The door direction as an integer value.</param>
        public RoomCoordinatesDTO((int, int)? position, (int, int)? size, int? doorDirection)
        {
            Position = position;
            Size = size;
            DoorDirection = doorDirection;
        }
    }
}