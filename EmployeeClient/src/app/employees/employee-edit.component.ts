import { Subject, takeUntil } from 'rxjs';
import { EmployeesService } from './employees.service';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { environment } from './../../environments/environment';
import { Employee } from './employee';
import { AuthService } from '../auth/auth.service';
import { BaseFormComponent } from '../base-form.component';

@Component({
  selector: 'app-employee-edit',
  templateUrl: './employee-edit.component.html',
  styleUrls: ['./employee-edit.component.scss']
})
export class EmployeeEditComponent extends BaseFormComponent implements OnInit {
// the view title
title?: string;
// the employee object to edit or create
employee?: Employee;
// the employee object id, as fetched from the active route:
// It's NULL when we're adding a new employee,
 // and not NULL when we're editing an existing one.
id?: number;
  constructor(private activatedRoute: ActivatedRoute,
    private router: Router,
    private http: HttpClient,
    public employeesService: EmployeesService
    ) {
      super();
    }

    ngOnInit() {
    this.form = new FormGroup({
      name: new FormControl('',Validators.required),
      age: new FormControl('',[Validators.required,Validators.pattern(/^\d+$/)]),
      sex: new FormControl('',Validators.required),
      job: new FormControl('',Validators.required),
      salary: new FormControl('',[Validators.required,Validators.pattern(/^\d+$/)])
  });
  this.loadData()
}
loadData() {
  // retrieve the ID from the 'id' parameter
  var idParam = this.activatedRoute.snapshot.paramMap.get('id');
   this.id = idParam ? +idParam : 0;
  if (this.id) {
    //Edit Mode

  // fetch the city from the server
  this.employeesService.getEmployee(this.id).subscribe(result => {
    console.log(result)
  this.employee = result;
  this.title = "Edit - " + this.employee.name;
  // update the form with the city value
  this.form.patchValue(this.employee);
  }, error => console.error(error));
  }
  else {
    // ADD NEW MODE
    this.title = "Create a new Employee";
    }
  }
  onSubmit() {
    var employee = (this.id) ? this.employee : <Employee>{};
  if (employee) {
    employee.name = this.form.controls['name'].value;
    employee.age = +this.form.controls['age'].value;
    employee.sex = this.form.controls['sex'].value;
    employee.job = this.form.controls['job'].value;
    employee.salary = +this.form.controls['salary'].value;
    if (this.id) {
      //Edit Mode
  console.log(employee)
  this.employeesService.put(employee)
  .subscribe(result => {
  console.log("City " + employee!.id + " has been updated.");
  this.router.navigate(['/employees']);
  }, error => console.error(error));
  }
  else {
    // ADD NEW mode
    this.employeesService.post(employee)
    .subscribe(result => {
    console.log("Employee has been created.");
    // go back to cities view
    this.router.navigate(['/employees']);
  }, error => console.error(error));
}
}
}
}

