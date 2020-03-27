import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Router, ActivatedRoute, NavigationEnd, RoutesRecognized } from '@angular/router';
import { debounceTime, distinctUntilChanged, map, filter } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { ComponentBase } from '../_shared/componentBase';
import { ApiClientService } from '../_services/api-client.service';
import { User } from '../_models/user';
import { AppConstants } from '../_shared/appConstants';

const commonNames = ['Justin', 'Bob', 'Barry', 'Calvin', 'Don', 'Chris']

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})
export class UserListComponent extends ComponentBase implements OnInit {

  public users: User[]
  public dataLoaded = false
  public userToImpersonate: any
  public showErrorToast = false
  public isFilterCollapsed = false
  public filterModel: any
  public listView: boolean

  @ViewChild('impersonationComplete', null) 
  private impersonationCompleteRef: ElementRef

  @ViewChild('impersonationConfirm', null) 
  private impersonationConfirmRef: ElementRef

  constructor(
    private readonly apiClientService: ApiClientService,
    private readonly router: Router,
    private readonly route: ActivatedRoute,
    private readonly modalService: NgbModal
  ) { super() }
  
  ngOnInit() {

    this.listView = this.isListView()

    const routerEvents$ = this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    )

    routerEvents$.subscribe(() => this.listView = this.isListView());

    this.apiClientService.getUsers().subscribe(users => { 
      this.users = users
      this.dataLoaded = true
    })
  }

  private isListView = () => {
    const urlFragment = this.route.snapshot.fragment;
    return !urlFragment || urlFragment === 'listView'
  }

  public onImpersonateClicked(user: any) {
    
    this.userToImpersonate = user

    this.modalService.open(this.impersonationConfirmRef, {ariaLabelledBy: 'modal-basic-title'}).result.then(
      this.handleImpersonationConfirmed, 
      () => {}
    );
  }

  private handleImpersonationConfirmed = () => {

    const {id} = this.userToImpersonate

    this.apiClientService.impersonate(id).subscribe(
      data => {
        localStorage.setItem(AppConstants.UserToken, data.token)
        this.modalService.open(this.impersonationCompleteRef).result.then(this.handleImpersonationLinkClicked)
      }, 
      error => this.showErrorToast = error.status === 403
    )
  }

  private handleImpersonationLinkClicked = (linkType: string) => {
    linkType === 'Reload' ? window.location.reload() : this.router.navigate([this.LocationsPage])
  }

  public onFirstNameFilterClicked() {
    this.users = this.users.filter(oo => oo.firstName.toLowerCase() == this.filterModel.toLowerCase())
  }

  public searchUsers = (text$: Observable<string>) => {
    return text$.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      map(term => term.length < 2 ? [] : commonNames.filter(v => v.toLowerCase().indexOf(term.toLowerCase()) > -1).slice(0, 10))
    )
  }
}
