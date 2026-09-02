import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { BondDetails } from '../models/bond-details.model';

@Injectable()
export class BondDetailsApiService {
  private readonly http = inject(HttpClient);

  get(id: number): Observable<BondDetails> {
    return this.http.get<BondDetails>(`/Bond/Details/${id}`);
  }

  create(data: BondDetails): Observable<BondDetails> {
    return this.http.post<BondDetails>('/Bond/Details', data);
  }

  update(data: BondDetails): Observable<BondDetails> {
    const { id, ...body } = data;
    return this.http.put<BondDetails>(`/Bond/Details/${id}`, body);
  }
}