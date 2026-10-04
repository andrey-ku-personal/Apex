import { Component, computed, inject, resource, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { Router } from '@angular/router';
import { firstValueFrom } from 'rxjs';

import { BondListTable } from './components/bond-list-table/bond-list-table';
import { BondListItem } from './models/bond-list-item';
import { BaseListFilter } from '../../../../shared/models/base-list-filter.model';
import { PageDataResponse } from '../../../../shared/models/page-data-response.model';
import { BondListApiService } from './services/bond-list-api.service';
import { pluralize } from '../../../../shared/utils/pluralize';

@Component({
  selector: 'app-bond-page',
  imports: [MatButtonModule, MatIconModule, BondListTable],
  providers: [BondListApiService],
  templateUrl: './bond-page.html',
  styleUrl: './bond-page.scss',
})
export class BondPage {
  private readonly apiService = inject(BondListApiService);
  private readonly router = inject(Router);

  protected readonly pageSize = 20;
  protected readonly pageNumber = signal(0);

  readonly bondsResource = resource<PageDataResponse<BondListItem>, BaseListFilter>({
    params: () => ({ pageNumber: this.pageNumber(), pageSize: this.pageSize, isAscending: true }),
    loader: ({ params }) => firstValueFrom(this.apiService.getList(params)),
  });

  protected readonly items = computed(() => this.bondsResource.value()?.data ?? []);
  protected readonly totalCount = computed(() => this.bondsResource.value()?.totalCount ?? 0);
  protected readonly loading = computed(() => this.bondsResource.isLoading());

  protected readonly assetsLabel = computed(() =>
    pluralize(this.totalCount(), ['актив', 'актива', 'активов']),
  );

  protected createBond(): void {
    this.router.navigate(['/bond', 0]);
  }

  protected openBond(id: number): void {
    this.router.navigate(['/bond', id]);
  }

  protected changePage(page: number): void {
    this.pageNumber.set(page);
  }
}
