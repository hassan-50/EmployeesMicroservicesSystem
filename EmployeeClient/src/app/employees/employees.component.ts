import { HttpClient } from '@angular/common/http';
import { Component, OnInit, ViewChild, OnDestroy } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Employee } from './employee';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { Observable } from 'rxjs/internal/Observable';
import { EmployeesService } from './employees.service';
import { AuthService } from '../auth/auth.service';
import { Subject, takeUntil } from 'rxjs';

@Component({
  selector: 'app-employees',
  templateUrl: './employees.component.html',
  styleUrls: ['./employees.component.scss']
})
export class EmployeesComponent implements OnInit, OnDestroy {
  // the employee check to see if admin or not
  isAdmin: boolean = false;
  private destroySubject = new Subject();
  public employees!: MatTableDataSource<Employee>;
  // public result: Observable<Employee[] | null>;
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  public displayedColumns: string[] = ['id', 'name', 'age', 'sex', 'job', 'salary','delete'];
  constructor(private http: HttpClient,public employeesService: EmployeesService, private authService: AuthService) {

    this.employeesService.result.subscribe(result => {
      console.log(result)
      this.employees = new MatTableDataSource<Employee>(result);
      this.employees.paginator = this.paginator;
    }, error => console.error(error));

    this.authService.authStatus
 .pipe(takeUntil(this.destroySubject))
 .subscribe(result => {
 this.isAdmin = result;
 this.checkIfAdmin(result)
})
  }

  ngOnInit(): void {
    this.isAdmin = this.authService.isAdmin()
    this.checkIfAdmin(this.authService.isAdmin())
    this.employeesService.startConnection();
    this.employeesService.addDataListeners();
    this.getEmployees();
  }
  getEmployees(){
    this.employeesService.get()
    .subscribe(result => {
      console.log(result)
      this.employees = new MatTableDataSource<Employee>(result);
      this.employees.paginator = this.paginator;
    }, error => console.error(error));
  }

  deleteEmployee(emp: Employee){
    if(confirm("Are you sure to delete "+emp.name)) {
    this.employeesService.delete(emp.id).subscribe(result => {
    console.log("Employee " + emp.name + " has been deleted.");
    this.getEmployees();

    }, error => console.error(error));
  }
}
ngOnDestroy() {
  this.destroySubject.next(true);
  this.destroySubject.complete();
  }
  checkIfAdmin(isAdmin: boolean){
    console.log(isAdmin)
    if(isAdmin == false)
    this.displayedColumns = ['id', 'name', 'age', 'sex', 'job', 'salary'];
    else
    this.displayedColumns = ['id', 'name', 'age', 'sex', 'job', 'salary','delete'];
  }
}

