import {Component, OnDestroy, OnInit} from '@angular/core';
import {SignalRService} from "../signal-r.service";
import {Router} from "@angular/router";
import {Subscription} from "rxjs";

@Component({
  selector: 'app-game',
  templateUrl: './game.component.html',
  styleUrls: ['./game.component.css']
})
export class GameComponent implements OnInit, OnDestroy{
  private leaveFromRoomSubscribe: Subscription;

  constructor(public gameService: SignalRService, private router: Router) { }

  ngOnInit() {
    if (!this.gameService.nowInRoom)
      this.router.navigate(["/"])

    this.leaveFromRoomSubscribe = this.gameService.leaveFromRoomEvent.subscribe(() => {
      this.router.navigate(["/"])
    })
  }

  isEnabled(column: number) {
    console.log(column)
    return !this.gameService.nowInRoom.isFinished && this.gameService.nowInRoom.isStarted && column == 0 && this.gameService.nowInRoom.isFirstPlayerStep == this.gameService.isFirstPlayer
  }

  makeStep(row: number, column: number) {
    this.gameService.makeStep(row, column)
  }

  leave() {
    this.gameService.leaveFromRoom()
  }

  ngOnDestroy(): void {
    if (this.leaveFromRoomSubscribe)
      this.leaveFromRoomSubscribe.unsubscribe()
  }
}
