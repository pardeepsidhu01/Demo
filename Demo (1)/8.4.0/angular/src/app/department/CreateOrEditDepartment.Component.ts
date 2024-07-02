import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/app-component-base';
import { CreateOrEditDepartmentDto, DepartmentDto, DepartmentServiceProxy } from '@shared/service-proxies/service-proxies';

@Component({
    selector: 'createOrEditModal',
    templateUrl: './createOrEditDepartment.component.html'
})
export class CreateOrEditDepartmentModalComponent extends AppComponentBase implements OnInit {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    active = false;
    saving = false;
record:DepartmentDto[]=[];
    id:number;
    department:CreateOrEditDepartmentDto = new CreateOrEditDepartmentDto()
   
   
    constructor(
        injector: Injector,private _departmentServiceProxy: DepartmentServiceProxy,
    ) {
        super(injector);
    }

   
    ngOnInit(): void {

    }
    show(departmentId?: number): void {
        if (!departmentId) {
            this.department = new CreateOrEditDepartmentDto();
           
            this.active = true;
            this.modal.show();
        } else {
            this._departmentServiceProxy.getDepartmentForEdit(departmentId).subscribe(result => {
                this.department = result.createOrEditDepartmentDto
                this.active = true;
                this.modal.show();
            });
        }

    }
    close(): void {
        this.modal.hide();
    }
    save(): void {
        this.saving = true;
        this._departmentServiceProxy.createOrEdit(this.department)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.modalSave.emit(null);
                this.close();
            });
    }
}
