import { Component, Injector, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';
import { AppComponentBase } from '@shared/app-component-base';
import { DepartmentDto, DepartmentServiceProxy, DesignationDto, DesignationServiceProxy, EmployeeDto, EmployeeServiceProxy } from '@shared/service-proxies/service-proxies';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { CreateOrEditEmployeeModalComponent } from './CreateOrEditEmployee.component';


@Component({
    templateUrl: './employee.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})

export class EmployeeComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModalEmployee', { static: true }) createOrEditEmployeeModalComponent: CreateOrEditEmployeeModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    filterText = '';
    constructor(
        injector: Injector,
        private _router: Router,
        private _employeeServiceProxy: EmployeeServiceProxy

    ) {
        super(injector);
    }

    ngOnInit(): void {

    }


    getAllEmployee(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }
        this.primengTableHelper.showLoadingIndicator();
        this._employeeServiceProxy.getAll(
            this.filterText,
            this.primengTableHelper.getSorting(this.dataTable),
            this.primengTableHelper.getSkipCount(this.paginator, event),
            this.primengTableHelper.getMaxResultCount(this.paginator, event)
        ).subscribe(result => {
            this.primengTableHelper.totalRecordsCount = result.totalCount;
            this.primengTableHelper.records = result.items;
            this.primengTableHelper.hideLoadingIndicator();
        })
    }
    createOrEditComponent() {
        this.createOrEditEmployeeModalComponent.show();
    }
    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }
    delete(employee: EmployeeDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._employeeServiceProxy.delete(employee.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }
}