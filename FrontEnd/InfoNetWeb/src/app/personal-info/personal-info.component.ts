import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormControl, Validators} from '@angular/forms';
import { icon } from '@fortawesome/fontawesome-svg-core';
import { ModalDismissReasons, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { PagerService } from '../shared/pagerservice';
import { ServiceService } from '../shared/service.service';
@Component({
  selector: 'app-personal-info',
  templateUrl: './personal-info.component.html',
  styleUrls: ['./personal-info.component.css'],
  providers: [ServiceService,PagerService]
})
export class PersonalInfoComponent implements OnInit {
  //Pagination
  public pageNumber: number = 0;
  public pageSize: number = 5;
  public totalRows: number = 0;
  public pageStart: number = 0;
  public pageEnd: number = 0;
  public totalRowsInList: number = 0;
  public pageSizeList: any = [];
  // pager object
  public pager: any = {};
  // paged items
  public pagedItems: any = [];

  //variable
  active = 'list';
  listPersonalInfo: any=[];
  closeResult = '';
  pInfoForm = new FormGroup({
        userId: new FormControl(0),
        fullName: new FormControl('', Validators.required),
        cityId: new FormControl(0, Validators.required),
        countryId: new FormControl(0, Validators.required),
        dateOfBirth: new FormControl('', Validators.required),
        skilList:new FormControl([])
  });

  listCountry:any=[];
  listCity:any=[];
  listLanguageSkils:any=[];

	constructor(
    private modalService: NgbModal,
    private formBuilder: FormBuilder,
    private _dataService: ServiceService,
    private pagerService: PagerService
    ) {
            this.pageSizeList = this.pagerService.pageSize();
    }

  ngOnInit(): void {
    this.loadPersonalInfo(0,true);
    this.loadCountryAndCity();
    this.loadSkills();
  }
  public res:any;
  public _getPersonalInfoUrl:string='PersonalInfo/GetList';
  searchname:string='';
  loadPersonalInfo(pageIndex:number,isPaging:boolean){
    var param={pageNumber: pageIndex, pageSize: this.pageSize,values:this.searchname};
    this._dataService.getWithMultipleModel(this._getPersonalInfoUrl, param)
    .subscribe(response => {
        this.res = response;        
        if (this.res.list.length>0) {
          this.listPersonalInfo = this.res.list;
          this.totalRows = this.listPersonalInfo.length > 0 ? this.res.total : 0;
          this.totalRowsInList = this.listPersonalInfo.length;
      } else {
          this.listPersonalInfo = [];
          this.totalRows = 0;
          this.totalRowsInList = 0;
      }
      //paging info start  

      if (this.pageNumber == 0 || this.pageNumber == 1) {
          this.pageStart = 1;
          if (this.totalRowsInList < this.pageSize) {
              this.pageEnd = this.totalRowsInList;
          } else {
              this.pageEnd = this.pageSize;
          }
      } else {
          this.pageStart = (this.pageNumber - 1) * this.pageSize + 1;
          this.pageEnd = (this.pageStart - 1) + this.totalRowsInList;
      }
      //paging info end

      if (isPaging)
          this.setPagingToGetAll(pageIndex, false);
      else
          this.pagedItems = this.listPersonalInfo;


    }, error => {
        console.log(error);
    });
  }
//Set Page
setPagingToGetAll(page: number, isPaging: boolean) {
  this.pager = this.pagerService.getPager(this.totalRows, page, this.pageSize);
  if (isPaging) {
      this.loadPersonalInfo(page, false);
  }
  else {
      this.pagedItems = this.listPersonalInfo;
  }
}


  public _getSkillUrl:string='PersonalInfo/GetSkills';
  loadSkills(){
    var param={};
    this._dataService.getWithMultipleModel(this._getSkillUrl, param)
    .subscribe(response => {
        this.res = response;
        if (this.res.list != undefined) {
            this.listLanguageSkils = this.res.list;
        }

    }, error => {
        console.log(error);
    });
  }
  public allCity:any=[];
  public _getCountryAndCityUrl:string='PersonalInfo/GetCountryAndCity';
  loadCountryAndCity(){
    var param={};
    this._dataService.getWithMultipleModel(this._getCountryAndCityUrl, param)
    .subscribe(response => {
        this.res = response;
        if (this.res.listCountry != undefined) {
            this.listCountry = this.res.listCountry;
        }

    }, error => {
        console.log(error);
    });
  }
  onCountryChange(e:any){
    if (e.target.value>0 ) {
      let country=this.listCountry.filter((c:any)=>c.countryId==e.target.value)[0]; 
      if(country!=undefined){
        this.listCity=country.cities;
      }     
    } else {
      this.listCity=[];
    }
  }
  public pInfo:any;
  openModal(e:any,item:any,content:any){
    this.pInfo=item;
		this.modalService.open(content, { ariaLabelledBy: 'modal-basic-title' }).result.then(
			(result) => {
				this.closeResult = `Closed with: ${result}`;
			},
			(reason) => {
				this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
			},
		);
	}

	private getDismissReason(reason: any): string {
		if (reason === ModalDismissReasons.ESC) {
			return 'by pressing ESC';
		} else if (reason === ModalDismissReasons.BACKDROP_CLICK) {
			return 'by clicking on a backdrop';
		} else {
			return `with: ${reason}`;
		}
	}
  public isEdit:boolean=false;
  edit(e:any,item:any){    
    this.skillValidationMesg='';
    let country = this.listCountry.filter((c:any)=>c.countryId==item.countryId)[0]; 
    if(country != undefined){
      this.listCity=country.cities;
    }     
    this.active = 'entry';
    this.pInfoForm.setValue({
      userId: item.userId,
      fullName: item.fullName,
      countryId: item.countryId,
      cityId: item.cityId,
      dateOfBirth: item.dateOfBirth,
      skilList:[]
  });
  this.languageSkilList=item.skilList;
  this.listLanguageSkils.forEach((element:any) => {
    let exSkill =item.skilList.filter((sk:any)=>sk.skillId==element.skillId)[0];
    if(exSkill != undefined){
      element.isChecked=true;
    }  else{
      element.isChecked=false;
    }
  });
  this.isEdit=true;
  }
  deleteConfirmModal(e:any,item:any,confirmModal:any){
    this.pInfo=item;
		this.modalService.open(confirmModal, { ariaLabelledBy: 'modal-basic-title' }).result.then(
			(result) => {
				this.closeResult = `Closed with: ${result}`;
			},
			(reason) => {
				this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
			},
		);
  }
  _deleteDateUrl:string='PersonalInfo/Delete';
  deleteInformation(){
    let param={userId:this.pInfo.userId};
    this._dataService.deleteData(param, this._deleteDateUrl)
        .subscribe(response => {
            this.res = response;
            if (this.res.resdata.isSuccess) {                
                this.active='list';
                this.loadPersonalInfo(0,true);
            }
            else {
                
            }
        }, error => {           
            console.log(error);
        });
  }
  public files:any=[];
  public _submitPersonalInfoUrl:string='PersonalInfo/Save';
  public _updatePersonalInfoUrl:string='PersonalInfo/Update';
  validateSkill(){
    let isValid=true;
    var skill=this.languageSkilList.filter((x:any)=>x.isChecked==true)[0];
    if(skill==undefined){
      isValid=false;
    }
    return isValid;
  }
  
  public skillValidationMesg:string='';
  onSubmit(){
    
   if(!this.validateSkill()){
       this.skillValidationMesg='Please select at least on skill.';
   }else{
    this.skillValidationMesg='';
   }

    if(this.languageSkilList.length>0){
      this.pInfoForm.patchValue({
        skilList:this.languageSkilList
      });
    }
    var jsonTktData = JSON.stringify(this.pInfoForm.value);
        var formData = new FormData();
        formData.append('personalInfo', jsonTktData);
        
        this.files.forEach((item:any, index:number) => {
          formData.append('attachedFile', item, item.name);
      });
      if(this.isEdit){
        this._dataService.updateFormWithFormData(formData, this._updatePersonalInfoUrl)
        .subscribe(response => {
            this.res = response;
            if (this.res.resdata.isSuccess) {              
               this.skillValidationMesg='';
              
                this.files = [];
                this.active='list';
                this.loadPersonalInfo(0,true);
            }
            else {                
              
            }
        }, error => {
           
            console.log(error);
            this.files = [];
        });
      }else{
        this._dataService.saveFormWithFormData(formData, this._submitPersonalInfoUrl)
        .subscribe(response => {
            this.res = response;
            if (this.res.resdata.isSuccess) {
               
                this.files = [];
                this.active='list';
                this.loadPersonalInfo(0,true);
            }
            else {
              
            }
        }, error => {
           
            console.log(error);
            this.files = [];
        });
      }      
}
download(item:any) {   
    var src = this._dataService.apiHost + "FileLoader/" + item.fileName;  
    this.downloadURI(src, item.fileName);
}

downloadURI(uri:any, name:any) {
    let link = document.createElement("a");
    link.download = name;
    link.href = uri;
    link.target='_blank';
    link.click();
}
  reset() {
    this.pInfoForm.setValue({
        userId: 0,
        fullName: '',
        countryId: 0,
        cityId: 0,
        dateOfBirth: '',
        skilList:[]
    });
    this.listLanguageSkils.forEach((element:any) => {     
        element.isChecked=false;     
    });
    this.isEdit=false;    
    this.skillValidationMesg='';
}
onCheckSkillChange(e:any,item:any){  
  if(e.currentTarget.checked){
    var skill=this.languageSkilList.filter((x:any)=>x.skillId==item.skillId)[0];
    if(skill==undefined){
      this.languageSkilList.push(item);
      this.skillValidationMesg = '';
    }    
  }else{
    var skill=this.languageSkilList.filter((x:any)=>x.skillId==item.skillId)[0];
    if(skill != undefined)
    {
    this.languageSkilList.splice(this.languageSkilList.indexOf(skill), 1);
    }
    if(!this.validateSkill()){
       this.skillValidationMesg='Please select at least one skill.';
    }else{
      this.skillValidationMesg = '';
    }
  }
}
showCommaSeparatedSkills(item:any){
  let skills='';
  if(item!=undefined){
    if (item.skilList.length>0) {
       let skillArr =item.skilList.map((x:any)=>x.skillName);
      skills=skillArr.join(',');
    }
  }  
  return skills;
}
public languageSkilList:any=[];
public fileTypes: any = [ "pdf", "doc", "docx"];
public errorMsg:string='';
onFileChanges(event:any) {
  if (event.target.files.length > 0) {
      var nFile = 0;
      for (var i = 0; i < event.target.files.length; i++) {
          let file = event.target.files[i];

          var arryext = file.name.split(".");
          var ext = arryext[arryext.length - 1];
          var extlwr = ext.toLowerCase();
          var fileIndex = this.fileTypes.indexOf(extlwr);
          var fileSize = file.size / 1024 / 1024; // in MB
          if (fileSize > 4) {
              this.errorMsg= 'File size exceeds 4 MB';

          } else if (fileIndex === -1) {
              this.errorMsg= 'File type not supported. Valid file types are ' + this.fileTypes;
          } else {            
            this.errorMsg='';
              var fileData = {
                  fileName: file.name,
                  fileData: '',
                  extension: ext.toLowerCase()
              };
              this.files.push(file);
              nFile += 1;
          }
      }

      if (nFile !== event.target.files.length) {
        this.errorMsg= 'Invalid file';
      }
  }
}
}
