import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { BondListItem } from '../models/bond-list-item';
import { BaseListFilter } from '../../../../../shared/models/base-list-filter.model';
import { PageDataResponse } from '../../../../../shared/models/page-data-response.model';

@Injectable()
export class BondListApiService {
  private readonly http = inject(HttpClient);

  getList(filter: BaseListFilter): Observable<PageDataResponse<BondListItem>> {
    return this.http.post<PageDataResponse<BondListItem>>('/Bond/Get/List', filter);
  }
}
