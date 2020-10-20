import {EventEmitter, Injectable} from '@angular/core';
import {HubConnection, HubConnectionBuilder, HubConnectionState} from "@aspnet/signalr";
import {RoomDto} from "../dto/signalrDtos";

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  private hubConnection: HubConnection
  public rooms: RoomDto[]
  public nowInRoom: RoomDto
  public isFirstPlayer: boolean
  public connectionId: string
  public isConnected: boolean


  public goToRoomEvent: EventEmitter<string> = new EventEmitter<string>()
  public leaveFromRoomEvent: EventEmitter<void> = new EventEmitter()

  constructor() {
    this.buildHubConnection()
    this.startConnection()
  }

  public buildHubConnection() {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl("/gameHub")
      .build()
  }

  public startConnection() {
    this.hubConnection
      .start()
      .then(() => {
        console.log("Connected")
        this.registerSignalEvents()
        this.hubConnection.send("GetConnectionId")
        this.isConnected = this.hubConnection.state == HubConnectionState.Connected
      })
      .catch(err => {
        console.log("Connection error: " + err)

        setTimeout(() => {
          this.startConnection()
        }, 2000)
      })
  }

  public createRoom(name: string) {
    this.hubConnection.send("CreateRoom", name);
  }

  public connectToRoom(id: string) {
    this.hubConnection.send("ConnectToRoom", id)
  }

  public makeStep(row: number, column: number) {
    this.hubConnection.send("MakeStep", row, column)
  }

  public leaveFromRoom() {
    this.hubConnection.send("LeaveFromRoom")
  }

  private registerSignalEvents() {
    this.hubConnection.on("rooms", (x: RoomDto[]) => {
      this.rooms = x
    })
    this.hubConnection.on("newRoom", (room: RoomDto) => {
      this.rooms.push(room)
    })
    this.hubConnection.on("removeRoom", (roomId: string) => {
      this.rooms = this.rooms.filter(x => x.id != roomId)
    })
    this.hubConnection.on("getConnectionId", (connectionId: string) => {
      this.connectionId = connectionId
    })
    this.hubConnection.on("moveToRoom", (room: RoomDto) => {
      this.isFirstPlayer = room.playerOne == this.connectionId
      this.nowInRoom = room
      this.goToRoomEvent.emit(room.id)
    })
    this.hubConnection.on("roomUpdated", (room: RoomDto) => {
      this.nowInRoom = room
    })
    this.hubConnection.on("leaveFromRoom", () => {
      this.nowInRoom = null
      this.isFirstPlayer = null
      this.leaveFromRoomEvent.emit()
    })
  }
}
