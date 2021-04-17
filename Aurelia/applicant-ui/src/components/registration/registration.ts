import { ApplicantService } from "./../../services/applicant.service";
import { autoinject } from "aurelia-framework";
import {
  ValidationControllerFactory,
  ValidationRules,
} from "aurelia-validation";
import { DialogService } from "aurelia-dialog";
import { BootstrapFormRenderer } from "../bootstrap/bootstrap-form-renderer";
import { Prompt } from "../dialog/modals/my-modal";
import { Router } from "aurelia-router";

export interface ModelAttributes {
  title: string;
  errors: string[];
  reset: boolean;
}
export interface RegistrationFormAttributes {
  name: string;
  familyName: string;
  address: string;
  emailAddress: string;
  hired: boolean;
  age: string;
  countryOfOrigin: string;
}
@autoinject
export class Registration implements RegistrationFormAttributes {
  name = "";
  familyName = "";
  emailAddress = "";
  address = "";
  hired = false;
  age = "";
  countryOfOrigin = "";
  dialogService: DialogService;
  enableDisabledButton = true;
  message = "";
  router: Router;
  errorResponse!: string[];
  controller: any;
  constructor(
    controllerFactory: ValidationControllerFactory,
    dialogService: DialogService,
    router: Router,
    private applicantService: ApplicantService
  ) {
    this.controller = controllerFactory.createForCurrentScope();
    this.controller.addRenderer(new BootstrapFormRenderer());
    this.dialogService = dialogService;
    this.enableDisabledButton = true;
    this.router = router;

    ValidationRules.customRule(
      "is a number",
      (value, obj) =>
        value === null ||
        value === undefined ||
        Number.isInteger(Number(value)),
      `\${$displayName} must be a number.`
    );
    ValidationRules.customRule(
      "is in range",
      (value, obj) =>
        value === null || value === undefined || (value >= 21 && value <= 59),
      `\${$displayName} must be between 20 and 60.`
    );
    ValidationRules.ensure((a: Registration) => a.name)
      .required()
      .minLength(5)
      .ensure((a: Registration) => a.familyName)
      .required()
      .minLength(5)
      .ensure((a: Registration) => a.emailAddress)
      .required()
      .email()
      .ensure((a: Registration) => a.address)
      .required()
      .minLength(10)
      .ensure((a: Registration) => a.countryOfOrigin)
      .required()
      .ensure((a: Registration) => a.age)
      .required()
      .satisfiesRule("is a number")
      .satisfiesRule("is in range")
      .on(Registration);
  }

  submit(): void {
    this.errorResponse = [];
    this.message = "";
    const applicant = {
      name: this.name,
      familyName: this.familyName,
      address: this.address,
      emailAddress: this.emailAddress,
      hired: this.hired,
      countryOfOrigin: this.countryOfOrigin,
      age: this.age,
    };

    this.addApplicant(applicant);
  }

  activateResetButton(): void {
    const lengthOfStr = [
      this.name,
      this.familyName,
      this.address,
      this.emailAddress,
      this.hired.toString(),
      this.countryOfOrigin,
      this.age,
    ];
    const noEmptyStrings = lengthOfStr.every((s: string) => s.length > 0);
    if (noEmptyStrings) {
      this.enableDisabledButton = false;
    } else {
      this.enableDisabledButton = true;
    }
  }
  openModal(model: ModelAttributes): void {
    this.dialogService
      .open({ viewModel: Prompt, model: model, lock: false })
      .whenClosed((response) => {
        if (!response.wasCancelled) {
          if (model.reset) {
            this.name = "";
            this.familyName = "";
            this.emailAddress = "";
            this.address = "";
            this.age = "";
            this.countryOfOrigin = "";
            this.enableDisabledButton = true;
          }

          console.log("good");
        } else {
          console.log("bad");
        }
        console.log(response.output);
      });
  }
  async addApplicant(applicant: RegistrationFormAttributes): Promise<void> {
    const response = await this.applicantService.createApplicant(applicant);
    if (response.success) {
      this.router.navigate("confirmation");
    }
    // I don't😨
    if (response.status === 400) {
      const data = await response.json();
      if (data.message !== null) {
        this.message = data.message;
      }
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

    //   localStorage.setItem("userData", JSON.stringify(data));
  }
  reset(): void {
    this.openModal({
      errors: ["Are you sure ?"],
      title: "MESSAGE",
      reset: true,
    });
  }
}
