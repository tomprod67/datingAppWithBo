import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {AccountService} from "../_services/account.service";

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  //@Input() usersFormHomeComponent: any;                 // * Permet de faire un flux de données du component Parent vers Enfant
  @Output() cancelRegister = new EventEmitter();        // * Permet de faire un flux de données du component Enfant vers Parent
  model: any = {};

  constructor(private accountService: AccountService) { }

  ngOnInit(): void {
  }

  register() {
    this.accountService.register(this.model).subscribe(response => {
      console.log(response);
      this.cancel();
    }, error => {
      console.log(error)
    })
  }

  cancel(){
    this.cancelRegister.emit(false);
  }

}
