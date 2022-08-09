import { MatInputModule } from '@angular/material/input';
import { NgModule } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { ReactiveFormsModule } from '@angular/forms'
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
@NgModule({
 imports: [
 MatButtonModule,
 MatIconModule,
 MatToolbarModule
 ],
 exports: [
 MatButtonModule,
 MatIconModule,
 MatToolbarModule,
 MatTableModule,
 MatPaginatorModule,
 ReactiveFormsModule,
 MatFormFieldModule,
 MatInputModule,
 MatSelectModule
 ]
})
export class AngularMaterialModule { }
