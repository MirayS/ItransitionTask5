import {Component, OnDestroy, OnInit} from '@angular/core';
import {SignalRService} from "../signal-r.service";
import {Subscription} from "rxjs";
import {Router} from "@angular/router";

@Component({
  selector: 'app-rooms',
  templateUrl: './rooms.component.html',
  styleUrls: ['./rooms.component.css']
})
export class RoomsComponent implements OnInit, OnDestroy {
  private moveToRoomSubscribe: Subscription

  public roomName: string = ''
  public tags: string = ''

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
    this.gameService.createRoom(this.roomName.trim())
  }

  connectToRoom(id: string) {
    this.gameService.connectToRoom(id)
  }

}
