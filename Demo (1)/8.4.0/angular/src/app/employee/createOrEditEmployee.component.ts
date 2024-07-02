import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/app-component-base';
import { CreateOrEditDepartmentDto, CreateOrEditEmployeeDto, DepartmentDto, DepartmentServiceProxy, DesignationServiceProxy, EmployeeServiceProxy, Int32NameValueDto } from '@shared/service-proxies/service-proxies';

@Component({
    selector: 'createOrEditModalEmployee',
    templateUrl: './createOrEditEmployee.component.html'
})
export class CreateOrEditEmployeeModalComponent extends AppComponentBase implements OnInit {

    @ViewChild('createOrEditModalEmployee', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    active = false;
    saving = false;
record:DepartmentDto[]=[];
    id:number;
    employee:CreateOrEditEmployeeDto = new CreateOrEditEmployeeDto()
   departments: Int32NameValueDto[] = [];
   designations: Int32NameValueDto[] = [];
   department: any;
   designation: any;
    constructor(
        injector: Injector,private _employeeServiceProxy: EmployeeServiceProxy,private _designationServiceProxy: DesignationServiceProxy,private _departmentServiceProxy: DepartmentServiceProxy
    ) {
        super(injector);
    }

   
    ngOnInit(): void {
this._employeeServiceProxy.getNamesForDepartment().subscribe((result)=>{
    this.departments = result;
})
this._employeeServiceProxy.getNamesForDesignation().subscribe((result)=>{
    this.designations = result;
})
    }
    show(employeeId?: number): void {
        if (!employeeId) {
            this.employee = new CreateOrEditEmployeeDto();
           
            this.active = true;
            this.modal.show();
        } else {
            this._employeeServiceProxy.getEmployeeForEdit(employeeId).subscribe(result => {
                this.employee = result.createOrEditEmployeeDto
                this.active = true;
                this.modal.show();
            });
        }

    }

    close(): void {
        this.modal.hide();
    }
    save(): void {
        this.employee.departmentId = this.department;
        this.employee.designationId = this.designation;
        this.saving = true;
        this._employeeServiceProxy.createOrEdit(this.employee)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
                this.modalSave.emit(null);
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
            });
    }
}
