import { Component, Injector, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';
import { AppComponentBase } from '@shared/app-component-base';
import { DepartmentDto, DepartmentServiceProxy, DesignationDto, DesignationServiceProxy } from '@shared/service-proxies/service-proxies';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { CreateOrEditDesignationModalComponent } from './CreateOrEditDesignation.component';


@Component({
    templateUrl: './designation.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})

export class DesignationComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModalDesignation', { static: true }) createOrEditDesignationModalComponent: CreateOrEditDesignationModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    filterText = '';
    constructor(
        injector: Injector,
        private _router: Router,
        private _designationServiceProxy: DesignationServiceProxy

    ) {
        super(injector);
    }

    ngOnInit(): void {

    }


    getAllDesignation(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }
        this.primengTableHelper.showLoadingIndicator();
        this._designationServiceProxy.getAll(
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
    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }
    createOrEditComponent() {
        this.createOrEditDesignationModalComponent.show();
    }
    delete(designation: DesignationDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._designationServiceProxy.delete(designation.id)
                        .subscribe(() => {
                            this.reloadPage()
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }
}