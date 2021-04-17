import { ModelAttributes } from './../registration/registration';
import { ApplicantService } from "../../services/applicant.service";
import { autoinject } from "aurelia-framework";
import { Prompt } from 'components/dialog/modals/my-modal';
import { DialogService } from 'aurelia-dialog';

export interface ApplicantAttributes {
  id: number;
  name: string;
  familyName: string;
  emailAddress: string;
  address: string;
  hired: boolean;
  age: number;
  countryOfOrigin: string;
  applicants: [];
  canEdit: boolean;
}
interface countButtonAttributes {
  button: number;
  status: boolean;
  applicantsCount: number;
}
@autoinject
export class Applicants {
  name = "";
  familyName = "";
  emailAddress = "";
  address = "";
  hired = false;
  age = "";
  countryOfOrigin = "";
  applicants: ApplicantAttributes[];
  response!: "";
  count: countButtonAttributes[];
  pageNumber: 1 | undefined;
  errorResponse: string[];
  dialogService: DialogService;
  constructor(private applicantService: ApplicantService, dialogService: DialogService) {
    this.applicantService = applicantService;
    this.applicants = [];
    this.count = [];
    this.errorResponse = [];
    this.dialogService = dialogService;
  }
  openModal(model: ModelAttributes): void {
    this.dialogService
      .open({ viewModel: Prompt, model: model, lock: false })
      .whenClosed((response: any) => {
        if (!response.wasCancelled) {
          console.log("good");
        } else {
          console.log("bad");
        }
        console.log(response.output);
      });
  }
  async activate(): Promise<void> {
    await this.getApplicants(1);
  }
  async getApplicants(page: number): Promise<void> {
    
    const { data, count, perPage,  pageNumber, } = await this.applicantService.getApplicants(page);

    data?.forEach((element: ApplicantAttributes) => {
      element.canEdit = false;
      this.applicants.push(element);
    });
    this.pagination(count, perPage, pageNumber);
  }
  editApplicant(applicant: ApplicantAttributes): void {
    applicant.canEdit = true;
  }
  async saveApplicant(applicant: ApplicantAttributes): Promise<void> {
    applicant.canEdit = false;
    applicant.age = Number(applicant.age);
    applicant.hired = this.hired;

   const res = await this.applicantService.updateApplicant(applicant);
   const data = await res.json();
   if (data.errors !== null) {
    data.errors?.forEach(({ propertyValue }: any) => {
      this.errorResponse.push(propertyValue);
    });

    this.openModal({
      errors: this.errorResponse,
      title: "ERROR",
      reset: false,
    });
  }
  }
  async deleteApplicant({ id }: ApplicantAttributes): Promise<void> {
    this.applicants = this.applicants.filter((a) => a.id !== id);

    await this.applicantService.deleteApplicant(id);
  }
  paginatedButton(button: countButtonAttributes): void {
    this.count.forEach((x) => {
      if (x.button === button.button) {
        x.status = !x.status;
      }
    });
    this.applicants = [];
    this.getApplicants(button.button);
    this.pagination(button.applicantsCount, 6, button.button);
  }
  pagination(count: number, perPage: number, pageNumber: number): void {
    this.count = [];
    let j = 1;
    const counter = Math.floor(count / perPage);
    const remainingRecords = count % perPage;
    while (j <= counter) {
      const countButtonAttributes = {
        button: j,
        status: j === pageNumber ? true : false,
        applicantsCount: count,
      };
      this.count.push(countButtonAttributes);
      j++;
    }
    if (remainingRecords !== 0) {
      const countButtonAttributes = {
        button: j,
        status: j === pageNumber ? true : false,
        applicantsCount: count,
      };
      this.count.push(countButtonAttributes);
    }
  }
}
