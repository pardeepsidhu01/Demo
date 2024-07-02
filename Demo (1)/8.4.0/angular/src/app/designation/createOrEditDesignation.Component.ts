import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/app-component-base';
import { CreateOrEditDepartmentDto, CreateOrEditDesignationDto, DepartmentDto, DepartmentServiceProxy, DesignationServiceProxy } from '@shared/service-proxies/service-proxies';

@Component({
    selector: 'createOrEditModalDesignation',
    templateUrl: './createOrEditDesignation.component.html'
})
export class CreateOrEditDesignationModalComponent extends AppComponentBase implements OnInit {

    @ViewChild('createOrEditModalDesignation', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    active = false;
    saving = false;
record:DepartmentDto[]=[];
    id:number;
    designation:CreateOrEditDesignationDto = new CreateOrEditDesignationDto()
   
   
    constructor(
        injector: Injector,private _designationServiceProxy: DesignationServiceProxy,
    ) {
        super(injector);
    }

   
    ngOnInit(): void {

    }
    show(designationId?: number): void {
        if (!designationId) {
            this.designation = new CreateOrEditDesignationDto();
           
            this.active = true;
            this.modal.show();
        } else {
            this._designationServiceProxy.getDepartmentForEdit(designationId).subscribe(result => {
                this.designation = result.createOrEditDesignationDto
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
        this._designationServiceProxy.createOrEdit(this.designation)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
                this.modalSave.emit(null);
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
            });
    }
}
