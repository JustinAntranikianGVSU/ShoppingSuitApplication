import { ComponentBase } from './componentBase';
import * as _ from 'lodash';

export abstract class CheckBoxComponentBase extends ComponentBase {

  private static readonly columnChunks = 4
      
  protected mapToCheckboxChunks = <T>(data: T[], ids: any[]): T[][] => {
    const mapToCheckboxFunc = this.mapToCheckboxItem(ids)
    const checkboxes = data.map(mapToCheckboxFunc);
    const sizeForEachArray = Math.ceil(checkboxes.length / CheckBoxComponentBase.columnChunks)  
    return _.chunk(checkboxes, sizeForEachArray)
  }

  private mapToCheckboxItem = (ids: any[]) => (item: any) =>
  ({
    ...item,
    isChecked: ids.includes(item.id)    
  })

  protected extractCheckedIds = (chunkedData: any) => {
    return _.flatten(chunkedData).filter(oo => oo.isChecked).map(oo => oo.id)
  }

}