import { LoginComponent } from './auth/login.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EmployeeEditComponent } from './employees/employee-edit.component';
import { EmployeesComponent } from './employees/employees.component';
import { HomeComponent } from './home/home.component';
import { AuthGuard } from './auth/auth.guard';

const routes: Routes = [
{ path: '', component: HomeComponent, pathMatch: 'full' },
{ path: 'employee/:id', component: EmployeeEditComponent,canActivate: [AuthGuard] },
{ path: 'employee', component: EmployeeEditComponent,canActivate: [AuthGuard]  },
{ path: 'employees', component: EmployeesComponent,canActivate: [AuthGuard] },
{ path: 'login', component: LoginComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
