import {Component, OnDestroy, OnInit} from '@angular/core';
import {SignalRService} from "../signal-r.service";
import {Subscription} from "rxjs";
import {Router} from "@angular/router";
import {RoomDto} from "../../dto/signalrDtos";

@Component({
  selector: 'app-rooms',
  templateUrl: './rooms.component.html',
  styleUrls: ['./rooms.component.css']
})
export class RoomsComponent implements OnInit, OnDestroy {
  private moveToRoomSubscribe: Subscription

  public roomName: string = ''
  public tags = []
  public filters = []

  constructor(public gameService: SignalRService, private router: Router) { }

  ngOnInit() {
    this.moveToRoomSubscribe = this.gameService.goToRoomEvent.subscribe((roomId: string) => {
      console.log(roomId)
      this.router.navigate(["/game", roomId])
    })
  }

  ngOnDestroy(): void {
    if (this.moveToRoomSubscribe)
      this.moveToRoomSubscribe.unsubscribe()
  }

  createRoom() {
    this.gameService.createRoom(this.roomName.trim(), this.tags.map(x => x.value))
  }

  connectToRoom(id: string) {
    this.gameService.connectToRoom(id)
  }

  getFilteredRooms(): RoomDto[] {
    if (this.filters.length == 0)
      return this.gameService.rooms

    return this.gameService.rooms.filter((room => {
      console.log(room)
      console.log(room.tags.some(x => this.filters.some(y => y == x)))

      return room.tags.some(x => this.filters.some(y => y.value == x))
    }))
  }

}
