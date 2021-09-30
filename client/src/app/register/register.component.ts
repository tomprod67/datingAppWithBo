import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {AccountService} from "../_services/account.service";
import {ToastrService} from "ngx-toastr";

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  //@Input() usersFormHomeComponent: any;                 // * Permet de faire un flux de données du component Parent vers Enfant
  @Output() cancelRegister = new EventEmitter();        // * Permet de faire un flux de données du component Enfant vers Parent
  model: any = {};

  constructor(private accountService: AccountService, private toastr: ToastrService) { }

  ngOnInit(): void {
  }

  register() {
    this.accountService.register(this.model).subscribe(response => {
      console.log(response);
      this.cancel();
    }, error => {
      console.log(error)
      this.toastr.error(error.error)
    })
  }

  cancel(){
    this.cancelRegister.emit(false);
  }

}
