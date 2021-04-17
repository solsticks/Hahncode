import { RegistrationFormAttributes } from "./../components/registration/registration";
import { HttpClient, json } from "aurelia-fetch-client";
import { ApplicantAttributes } from "../components/applicants/applicants";

interface PaginatedResults {
  name: string;
  data: ApplicantAttributes[];
  count: number;
  perPage: number;
  pageNumber: number;
}
export class ApplicantService {
  static inject = [HttpClient];
  constructor(private http: HttpClient) {
    http.configure((config) => {
      config
        .useStandardConfiguration()
        .withBaseUrl("https://localhost:5001/api");
    });
    this.http = http;
  }
  async createApplicant(applicant: RegistrationFormAttributes): Promise<any> {
    const config = {
      headers: {
        "Content-Type": "application/json",
      },
      method: "POST",
      body: JSON.stringify(applicant),
    };
    try {
      const response = await this.http.fetch("/applicant", config);
      const result = await response.json();
      return result;
    } catch (error) {
      return error;
    }
  }
  async getApplicants(page: number): Promise<PaginatedResults> {
    try {
      const response = await this.http.fetch(`/applicant?pageNumber=${page}&perPage=6`);
      const applicants = await response.json();
      return applicants;
    } catch (error) {
      return error;
    }
  }
  async updateApplicant(applicant: ApplicantAttributes): Promise<any> {
    const id: number = applicant.id;
    const config = {
      headers: {
        "Content-Type": "application/json",
      },
      method: "PUT",
      body: JSON.stringify(applicant),
    };
    try {
      const response = await this.http.fetch(`/applicant/${id}`, config);
      const result = await response.json();
      return result;
    } catch (error) {
      return error;
    }
  }
  async deleteApplicant(id: number): Promise<unknown> {
    const config = {
      headers: {
        "Content-Type": "application/json",
      },
      method: "DELETE",
    };
    try {
      const response = await this.http.fetch(`/applicant/${id}`, config);
      const result = await response.json();
      return result;
    } catch (error) {
      return error;
    }
  }
}
