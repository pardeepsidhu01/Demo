import { Component, EventEmitter, Injector, OnInit, Output, ViewChild, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';
import { AppComponentBase } from '@shared/app-component-base';
import { DepartmentDto, DepartmentServiceProxy } from '@shared/service-proxies/service-proxies';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { CreateOrEditDepartmentModalComponent } from './CreateOrEditDepartment.component';


@Component({
    templateUrl: './department.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})

export class departmentComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) createOrEditModal: CreateOrEditDepartmentModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    filterText = '';
    record: DepartmentDto[] = [];
    constructor(
        injector: Injector,
        private _router: Router,
        private _departmentServiceProxy: DepartmentServiceProxy

    ) {
        super(injector);
    }

    ngOnInit(): void {

    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }
    getAllDepartment(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }
        this.primengTableHelper.showLoadingIndicator();
        this._departmentServiceProxy.getAll(
            this.filterText,
            this.primengTableHelper.getSorting(this.dataTable),
            this.primengTableHelper.getSkipCount(this.paginator, event),
            this.primengTableHelper.getMaxResultCount(this.paginator, event)
        ).subscribe(result => {
            this.primengTableHelper.totalRecordsCount = result.totalCount;
            this.primengTableHelper.records = result.items;
            this.record = result.items;
            this.primengTableHelper.hideLoadingIndicator();
        })
    }
    createOrEditComponent() {
        this.createOrEditModal.show();
    }
    delete(department: DepartmentDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._departmentServiceProxy.delete(department.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }
}