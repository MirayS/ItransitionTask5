export class RoomDto {
  public id: string
  public name: string
  public playingField: number[][]
  public playerOne: string
  public playerTwo: string
  public isStarted: boolean
  public isFinished: boolean
  public isFirstPlayerWin: boolean
  public isFirstPlayerStep: boolean
  public tags: string[]

  /*
  Id = Guid.NewGuid(),
                Name = roomName,
                IsFinished = false,
                IsStarted = false,
                IsFirstPlayerStep = true,
                PlayerOne = creatorId,
                PlayerTwo = null,
                PlayingField = new List<int> {0, 0, 0, 0, 0, 0, 0, 0, 0},
                IsFirstPlayerWin = null,
   */
}
