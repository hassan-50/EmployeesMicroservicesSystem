import { Employee } from './employee';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import * as signalR from "@microsoft/signalr";
import { environment } from './../../environments/environment';
import { Observable, Subject, tap } from 'rxjs';
import { NgToastService } from 'ng-angular-popup';
@Injectable({
 providedIn: 'root'
})
export class EmployeesService {
 private hubConnection!: signalR.HubConnection;
 private _result: Subject<Employee[]> = new Subject<Employee[]>();
 public result = this._result.asObservable();
 constructor(private http: HttpClient, private toast: NgToastService) {
 }
 public startConnection() {
  this.hubConnection = new signalR.HubConnectionBuilder()
  .configureLogging(signalR.LogLevel.Information)
  .withUrl(environment.signalrUrl, { withCredentials:
 false, skipNegotiation:true , transport: signalR.HttpTransportType.WebSockets})
  .build();
  console.log("Starting connection...");
  this.hubConnection
  .start()
  .then(() => console.log("Connection started."))
  .catch((err : any) => console.log(err));
  this.updateData();
  }
  public addDataListeners() {
  this.hubConnection.on('Update', (msg:string) => {
  this.toast.success({detail: "Success Message", summary:msg})
  this.updateData();
  });

  this.hubConnection.on('Error', (msg:string) => {
    this.toast.error({detail: "Error Message", summary:msg})
    this.updateData();
    });
  }
  public updateData() {
  console.log("Fetching data...");
  this.http.get<Employee[]>(environment.firstGateway + 'Employees')
    .subscribe(result => {
      this._result.next(result);
      console.log(result);

    }, error => console.error(error));
  }
  public get(): Observable<Employee[]>{
    return this.http.get<Employee[]>(environment.firstGateway + 'Employees')
  }
  public getEmployee(id:number): Observable<Employee>{
   return this.http.get<Employee>(environment.firstGateway + 'employees/' + id)
  }
  public post(employee: Employee): Observable<string>{
    return this.http.post<string>(environment.secondGateway + 'employees', employee)
  }
  public put(employee: Employee) : Observable<string>{
  return this.http.put<string>(environment.secondGateway + 'employees/' + employee.id, employee)
  }
  public delete(id: number) : Observable<string>{
    return this.http.delete<string>(environment.secondGateway + 'employees/' + id)
  }
}
