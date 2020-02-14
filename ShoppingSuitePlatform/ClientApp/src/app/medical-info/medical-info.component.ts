import { Component, OnInit } from '@angular/core';
import { MedicalInfoService } from '../services/medical-info.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

class UserInfo {
  occupationId: number
  occupationSectorId: number
}

@Component({
  selector: 'app-medical-info',
  templateUrl: './medical-info.component.html',
  styleUrls: ['./medical-info.component.css']
})
export class MedicalInfoComponent implements OnInit {

  public occupations: any[] = []
  public allOccupations: any[] = []
  public occupationSectors: any[] = []
  public userInfo: UserInfo

  constructor(
    private medicalInfoService: MedicalInfoService,
    private modalService: NgbModal
  ) { }

  ngOnInit() {
    this.medicalInfoService.GetOccupations().subscribe((result: any[]) => {
      this.occupations = result
      this.allOccupations = result
    })

    this.medicalInfoService.GetOccupationSectors().subscribe((result: any[]) => {
      this.occupationSectors = result
    })

    this.userInfo = { 
      occupationId: 2,
      occupationSectorId: 1
    }
  }

  public open(content) {
    this.modalService.open(content, {ariaLabelledBy: 'modal-basic-title'}).result.then((result) => {
      console.log('Modal Closed !!');
    });
  }

  private getFormatUserInfo = (userInfo: UserInfo): UserInfo => ({
    ...userInfo,
    occupationId: parseInt(this.userInfo.occupationId.toString())
  })

  public onOccupationSectorChanged() {
    const sectorId = parseInt(this.userInfo.occupationSectorId.toString())
    this.occupations = this.allOccupations.filter(oo => oo.occupationSectorId === sectorId)
  }

  public onSaveUserInfoClicked() {

    const formattedInfo = this.getFormatUserInfo(this.userInfo)

    this.medicalInfoService.SaveUserInfo(formattedInfo).subscribe((response) => {
      console.log('POST Completed', response)
    })
  }

}
