import { HttpClient, HttpHeaders, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";

export interface RequestOptions {
    sendToken?: boolean; // Por defecto true
    params?: { [param: string]: string | number }; // Parámetros de query
    customHeaders?: { [header: string]: string }; // Headers extra
    isFormData?: boolean; // Para indicar si es FormData
    responseType?: "arraybuffer" | "blob" | "json" | "text"; // Nuevo campo
}

@Injectable({
    providedIn: "root",
})
export class HttpService {
    constructor(private http: HttpClient) { }

    private createHeaders(options?: RequestOptions, body?: any): HttpHeaders {
        let headers =
            body instanceof FormData
                ? new HttpHeaders()
                : new HttpHeaders({ "Content-Type": "application/json" });

        const token = localStorage.getItem("token");
        if (token) {
            headers = headers.set("Authorization", `Bearer ${token}`);
        }

        return headers;
    }

    private createParams(options?: RequestOptions): HttpParams {
        let httpParams = new HttpParams();
        if (options?.params) {
            Object.entries(options.params).forEach(([key, value]) => {
                httpParams = httpParams.set(key, value);
            });
        }
        return httpParams;
    }

    private createBody(body: any): any {
        if (typeof body === "string") return body;
        if (body instanceof FormData) return body;
        return JSON.stringify(body);
    }

    get<T>(url: string, options?: RequestOptions): Observable<T> {
        return this.http.get<T>(url, {
            headers: this.createHeaders(options, undefined),
            params: this.createParams(options),
            responseType: options?.responseType as "json",
        });
    }

    post<T>(url: string, body: any): Observable<T> {
        return this.http.post<T>(url, this.createBody(body), {
            headers: this.createHeaders(undefined, body),
            params: this.createParams(),
            responseType: "json",
        });
    }

    put<T>(url: string, body?: any, options?: RequestOptions): Observable<T> {
        return this.http.put<T>(url, this.createBody(body), {
            headers: this.createHeaders(options, body),
            params: this.createParams(options),
        });
    }

    patch<T>(url: string, body?: any, options?: RequestOptions): Observable<T> {
        return this.http.patch<T>(url, this.createBody(body), {
            headers: this.createHeaders(options, body),
            params: this.createParams(options),
        });
    }

    delete<T>(url: string, options?: RequestOptions): Observable<T> {
        return this.http.delete<T>(url, {
            headers: this.createHeaders(options),
            params: this.createParams(options),
        });
    }
}