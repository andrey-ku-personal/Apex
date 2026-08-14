import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { BondDetails } from '../models/bond-details.model';

export class BondDetailsApiService {
  private http = inject(HttpClient);

  get(id: number): Observable<BondDetails> {
    return this.http.get<BondDetails>(`/Bond/Details/${id}`);
  }

  upsert(data: BondDetails): Observable<BondDetails> {
    return this.http.put<BondDetails>('/Bond/Details/Upsert', data);
  }
}